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
        NotificationHelper.NotifySuccess("Model copied successfully.\nUse the context menu or /pastemodel while editing a model to paste it somewhere else.");
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
        if (copiedData == null)
        {
            NotificationHelper.WarnUser("No data to copy");
            return;
        }

        targetModelID = model.id;
        instance.StartCoroutine(BeginBuildModel(model).WrapToIl2Cpp());
    }

    private static IEnumerator BeginBuildModel(MVCubeModelBase model)
    {
        yield return instance.StartCoroutine(BuildModel(model, copiedData).WrapToIl2Cpp());

        targetModelID = -1;
        NotificationHelper.NotifySuccess("Model pasted successfully.");
    }


    [HarmonyPatch(typeof(MVWorldObjectClientManagerNetwork), "DestroyWO")]
    [HarmonyPrefix]
    private static void UnregisterWorldObject(int id)
    {
        if (id == targetModelID)
        {
            instance.StopAllCoroutines();
            NotificationHelper.NotifySuccess("Model copy was aborted.");
        }
    }
}