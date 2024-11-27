using BepInEx.Unity.IL2CPP.Utils.Collections;
using HarmonyLib;
using KogamaTools.Helpers;
using KogamaTools.Tools.Misc;
using UnityEngine;
using static System.Environment;
using static System.IO.Path;
using static KogamaTools.Helpers.ModelHelper;
namespace KogamaTools.Tools.Build;

internal enum ModelImporterState
{
    None,
    WaitingForModel,
    ImportInProgress
}

[HarmonyPatch]
internal class ModelImporter : MonoBehaviour
{
    private static ModelImporter instance = null!;
    private static ModelData data = null!;
    private static ModelImporterState state = ModelImporterState.None;
    private static int targetModelID;
    private void Awake()
    {
        instance ??= this;
        WOReciever.OnWORecieved += OnWORecieved;
    }

    private static void ResetState()
    {
        state = ModelImporterState.None;
        data = null!;
        targetModelID = -1;
    }

    internal static void ImportModel(string path)
    {
        ResetState();
        NotificationHelper.NotifyUser("Loading model data...");
        byte[] serializedData;
        try
        {
            serializedData = LoadModelData(path);
        }
        catch (Exception e)
        {
            NotificationHelper.NotifyError(e.Message);
            throw;
        }
        NotificationHelper.NotifySuccess("Model data loaded successfully.");

        data = DeSerializeModelData(serializedData);

        MVCubeModelBase targetModel = GetTargetModel();

        if (targetModel.id != 75579)
        {
            BeginImport(targetModel);
            return;
        }

        RequestCubeModel(data.Scale);
    }

    private static byte[] LoadModelData(string path)
    {
        if (!IsPathFullyQualified(path))
        {
            path = Combine(GetFolderPath(SpecialFolder.ApplicationData), KogamaTools.ModName, "Models", path);
        }

        if (!File.Exists(path))
        {
            throw new Exception($"File does not exist.");
        }

        byte[] data = File.ReadAllBytes(path);


        return data;
    }

    private static void BeginImport(MVCubeModelBase model)
    {
        targetModelID = model.id;
        state = ModelImporterState.ImportInProgress;
        instance.StartCoroutine(BuildModel(model, data).WrapToIl2Cpp());
    }
    private static void OnWORecieved(MVWorldObjectClient wo, int instigatorActorNr)
    {
        if (state == ModelImporterState.WaitingForModel && instigatorActorNr == MVGameControllerBase.Game.LocalPlayer.ActorNr)
        {
            if (TryGetModelFromWO(wo, out var model))
            {
                BeginImport(model);
            }
        }
    }

    [HarmonyPatch(typeof(MVWorldObjectClient), "Delete")]
    [HarmonyPrefix]
    private static void UnregisterWorldObject(MVWorldObjectClient __instance)
    {
        if (state == ModelImporterState.ImportInProgress && __instance.id == targetModelID)
        {
            instance.StopAllCoroutines();
            ResetState();
        }
    }
}

