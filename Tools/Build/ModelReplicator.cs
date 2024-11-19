using System.Collections;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using KogamaTools.Helpers;
using KogamaTools.Tools.Misc;
using MV.WorldObject;
using UnityEngine;
using static KogamaTools.Helpers.ModelHelper;

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

    private static void ResetState()
    {
        state = ModelReplicatorState.None;
        sourceModel = null!;
        destinationModel = null!;
    }

    private void Awake()
    {
        instance ??= this;

        ResetState();

        CustomContextMenu.AddButton(
            "Copy Model",
            wo => IsModelOwner(wo),
            wo => SetSourceModel(wo)
            );

        WOReciever.OnWORecieved += OnWORecieved;
    }


    private static void SetSourceModel(MVWorldObjectClient wo)
    {
        ResetState();

        if (GetModelFromWO(wo, out MVCubeModelBase model))
        {
            sourceModel = model;
            state = ModelReplicatorState.WaitingForTargetModel;
            NotificationHelper.NotifyUser($"Source model for copy set to {wo.id}. Press <N> to add the destination model.");
        };
    }

    private static void SetDestinationModel(MVWorldObjectClient wo)
    {
        if (GetModelFromWO(wo, out MVCubeModelBase model))
        {
            destinationModel = model;
            state = ModelReplicatorState.CopyInProgress;
        }
    }

    private static void OnWORecieved(MVWorldObjectClient root, int instigatorActorNr)
    {
        if (state == ModelReplicatorState.WaitingForTargetModel && instigatorActorNr == MVGameControllerBase.Game.LocalPlayer.ActorNr)
        {
            if (!MVGameControllerBase.WOCM.IsType(root.id, WorldObjectType.CubeModel))
                return;

            SetDestinationModel(root);
            instance.StartCoroutine(CopyModel(sourceModel, destinationModel).WrapToIl2Cpp());
        }
    }

    private static IEnumerator CopyModel(MVCubeModelBase source, MVCubeModelBase destination)
    {
        NotificationHelper.NotifyUser("The model copy process has started. You can delete the target model at any time to abort it.");

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

                Cube cube = source.GetCube(cubePos);

                if (!MVMaterialRepository.instance.IsMaterialUnlocked(new Il2CppStructArray<byte>(cube.faceMaterials)))
                {
                    NotificationHelper.WarnUser($"Replacing materials at {cubePos.ToString()}: Material is locked.");

                    cube = MakeCubeFromBytes(cube.byteCorners, DefaultMaterials);
                }

                AddCubeToModel(cubePos, cube, destination);

                yield return new WaitForSeconds(Mathf.Max(1f / 60f - Time.deltaTime, 0f));
            }
        }

        NotificationHelper.NotifySuccess("Model copied successfully.");
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