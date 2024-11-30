using System.Collections;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;
using static KogamaTools.Helpers.ModelHelper;

namespace KogamaTools.Tools.Build;


[HarmonyPatch]
internal class CopyPasteModel : MonoBehaviour
{
    private static ModelData copiedData = null!;
    private static CopyPasteModel instance = null!;
    private static int targetModelID = -1;

    private void Awake()
    {
        instance ??= this;

        CustomContextMenu.AddButton("Copy Model", wo => IsModelOwner(wo), wo => CopyModel(wo));
        CustomContextMenu.AddButton("Paste Model", wo => IsModel(wo), wo => PasteModel(wo));
    }

    internal static void CopyModel(MVWorldObjectClient wo)
    {
        if (TryGetModelFromWO(wo, out var model))
        {
            CopyModel(model);
        }
    }

    internal static void CopyModel(MVCubeModelBase model)
    {
        copiedData = GetModelData(model);
    }

    internal static void PasteModel(MVWorldObjectClient wo)
    {
        if (copiedData == null)
        {
            NotificationHelper.WarnUser("No data to copy");
            return;
        }

        if (TryGetModelFromWO(wo, out var model))
        {
            PasteModel(model);
        }
    }

    internal static void PasteModel(MVCubeModelBase model)
    {
        targetModelID = model.id;
        instance.StartCoroutine(BeginBuildModel(model).WrapToIl2Cpp());
    }

    private static IEnumerator BeginBuildModel(MVCubeModelBase model)
    {
        NotificationHelper.NotifyUser("The model copy process has started. You can delete the target model at any time to abort it.");
        yield return instance.StartCoroutine(BuildModel(model, copiedData).WrapToIl2Cpp());
        NotificationHelper.NotifySuccess("Model imported successfully.");
    }


    [HarmonyPatch(typeof(MVWorldObjectClient), "Delete")]
    [HarmonyPrefix]
    private static void UnregisterWorldObject(MVWorldObjectClient __instance)
    {
        if (__instance.id == targetModelID)
        {
            instance.StopAllCoroutines();
        }
    }
}