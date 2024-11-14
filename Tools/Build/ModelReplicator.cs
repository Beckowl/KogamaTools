using System.Collections;
using BepInEx.Unity.IL2CPP.Utils.Collections;
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
    private static ModelReplicator instance = null!;
    private static MVCubeModelBase sourceModel = null!;
    private static MVCubeModelBase destinationModel = null!;
    private static ModelReplicatorState state;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        ResetState();

        CustomContextMenu.AddButton(
            wo => CanCopyModel(wo),
            "Copy model",
            wo => SetSourceModel(wo)
            );

        WOReciever.OnWORecieved += OnWORecieved;
    }

    private static void ResetState()
    {
        state = ModelReplicatorState.None;
        sourceModel = null!;
        destinationModel = null!;
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

    private static void SetSourceModel(MVWorldObject wo)
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

    private static void SetDestinationModel(MVWorldObject wo)
    {
        MVCubeModelBase prototype = wo.Cast<MVCubeModelBase>();
        if (prototype != null)
        {
            destinationModel = prototype;
            state = ModelReplicatorState.CopyInProgress;
        }
    }

    private static void OnWORecieved(MVWorldObject root, int instigatorActorNr)
    {
        if (state == ModelReplicatorState.WaitingForTargetModel && instigatorActorNr == MVGameControllerBase.Game.LocalPlayer.ActorNr)
        {
            if (!MVGameControllerBase.WOCM.IsType(root.id, WorldObjectType.CubeModel))
            {
                return;
            }

            SetDestinationModel(root);
            instance.StartCoroutine(CopyModel(sourceModel, destinationModel).WrapToIl2Cpp());
        }
    }

    private static IEnumerator CopyModel(MVCubeModelBase source, MVCubeModelBase destination)
    {
        NotificationHelper.NotifyUser("The model copy process has started. You can delete the target model at any time to abort the process.");

        foreach (CubeModelChunk chunk in source.prototypeCubeModel.Chunks.Values)
        {
            var enumerator = chunk.cells.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (state != ModelReplicatorState.CopyInProgress)
                {
                    yield break;
                }

                IntVector cubePos = enumerator.Current.Key;
                AddCubeToModel(cubePos, source.GetCube(cubePos), destination);

                yield return new WaitForSeconds(Mathf.Max(1f / 60f - Time.deltaTime, 0f));
            }
        }

        NotificationHelper.NotifySuccess("Model copied successfully.");
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