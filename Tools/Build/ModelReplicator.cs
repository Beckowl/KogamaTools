using HarmonyLib;
using KogamaTools.Helpers;
using KogamaTools.Tools.Misc;
using MV.WorldObject;
using UnityEngine;

namespace KogamaTools.Tools.Build;

enum ModelReplicatorState
{
    None,
    WaitingForTargetModel,
    CopyInProgress
}

[HarmonyPatch]
internal class ModelReplicator : MonoBehaviour
{
    private static ModelReplicatorState state;
    private static MVCubeModelBase sourceModel = null!;
    private static MVCubeModelBase destinationModel = null!;
    private static Dictionary<IntVector, Cube> cubesToCopy = new();
    private static int cubeIndex = 0;

    private void Awake()
    {
        ResetState();

        CustomContextMenu.AddButton(
            wo => CanCopyModel(wo),
            "Copy model",
            wo => EnterCopyModel(wo)
            );

        WOReciever.OnWORecieved += OnWORecieved;
    }

    private static bool CanCopyModel(MVWorldObjectClient wo)
    {
        if (MVGameControllerBase.WOCM.IsType(wo.id, WorldObjectType.CubeModel))
        {
            MVCubeModelBase model = wo.Cast<MVCubeModelBase>();
            if (model != null)
            {
                return model.prototypeCubeModel.AuthorProfileID == MVGameControllerBase.Game.LocalPlayer.ProfileID;
            }
        }
        return false;
    }

    private static void EnterCopyModel(MVWorldObject wo)
    {
        ResetState();
        MVCubeModelBase prototype = wo.Cast<MVCubeModelBase>();
        if (prototype != null)
        {
            sourceModel = prototype;
            state = ModelReplicatorState.WaitingForTargetModel;
            NotificationHelper.NotifyUser($"Source model for copy set to {wo.id}. Press <N> to add the destination model.");
        }
    }

    private static void ResetState()
    {
        state = ModelReplicatorState.None;
        sourceModel = null!;
        destinationModel = null!;
        cubesToCopy = null!;
        cubeIndex = 0;
    }


    private static void OnWORecieved(MVWorldObject root, int instigatorActorNr)
    {
        if (state == ModelReplicatorState.WaitingForTargetModel && instigatorActorNr == MVGameControllerBase.Game.LocalPlayer.ActorNr)
        {
            if (!MVGameControllerBase.WOCM.IsType(root.id, WorldObjectType.CubeModel))
            {
                return;
            }

            MVCubeModelBase prototype = root.Cast<MVCubeModelBase>();
            if (prototype != null)
            {
                destinationModel = prototype;
                state = ModelReplicatorState.CopyInProgress;
                NotificationHelper.NotifyUser("The model copy process has started. You can delete the object to abort the process.");
            }
        }
    }

    private void Update()
    {
        if (state == ModelReplicatorState.CopyInProgress)
        {
            if (cubesToCopy == null)
            {
                cubesToCopy = GetCubesFromModel(sourceModel);
            }

            try
            {
                var kvp = cubesToCopy.ElementAt(cubeIndex);
                AddCubeToModel(kvp.Key, kvp.Value, destinationModel);
                cubeIndex++;
            }
            catch (Exception e)
            {
                NotificationHelper.NotifyError(e.ToString());
#if DEBUG
                KogamaTools.mls.LogInfo(cubesToCopy.Count() + "\t" + cubeIndex);
#endif
                ResetState();
            }


            if (cubeIndex >= cubesToCopy.Count)
            {
                ResetState();
                NotificationHelper.NotifySuccess("Model copied successfully.");
            }
        }
    }

    private static Dictionary<IntVector, Cube> GetCubesFromModel(MVCubeModelBase model)
    {
        Dictionary<IntVector, Cube> cubes = new Dictionary<IntVector, Cube>();
        Queue<IntVector> toVisit = new Queue<IntVector>();

        IntVector origin = new IntVector(0, 0, 0);
        toVisit.Enqueue(origin);

        while (toVisit.Count > 0 && cubes.Count < model.prototypeCubeModel.CubeCount)
        {
            IntVector currentPos = toVisit.Dequeue();

            if (cubes.ContainsKey(currentPos)) continue;

            Cube cube = model.GetCube(currentPos);
            if (cube != null)
            {
                cubes[currentPos] = cube;

                toVisit.Enqueue(new IntVector(currentPos.x + 1, currentPos.y, currentPos.z));
                toVisit.Enqueue(new IntVector(currentPos.x - 1, currentPos.y, currentPos.z));
                toVisit.Enqueue(new IntVector(currentPos.x, currentPos.y + 1, currentPos.z));
                toVisit.Enqueue(new IntVector(currentPos.x, currentPos.y - 1, currentPos.z));
                toVisit.Enqueue(new IntVector(currentPos.x, currentPos.y, currentPos.z + 1));
                toVisit.Enqueue(new IntVector(currentPos.x, currentPos.y, currentPos.z - 1));
            }
        }

        return cubes;
    }

    private static void AddCubeToModel(IntVector position, Cube cube, MVCubeModelBase target)
    {
        if (target.ContainsCube(position))
        {
            target.RemoveCube(position);
        }

        target.AddCube(position, cube);
        target.HandleDelta();
    }

    [HarmonyPatch(typeof(MVWorldObjectClient), "Delete")]
    [HarmonyPrefix]
    private static void UnregisterWorldObject(MVWorldObjectClient __instance)
    {
        if (state == ModelReplicatorState.CopyInProgress && __instance.id == destinationModel.id)
        {
            ResetState();
        }
    }
}