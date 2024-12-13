using System.Collections;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using HarmonyLib;
using KogamaTools.Tools.Misc;
using UnityEngine;
using static System.Environment;
using static System.IO.Path;
using static KogamaTools.Helpers.ModelHelper;
using static KogamaTools.Helpers.NotificationHelper;

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

    internal static void ImportModel(string path)
    {
        ResetState();
        NotifyUser("Loading model data...");
        byte[] serializedData;
        try
        {
            serializedData = LoadModelData(path);
        }
        catch (Exception e)
        {
            NotifyError(e.Message);
            throw;
        }

        NotifySuccess("Model data loaded successfully.");

        data = DeSerializeModelData(serializedData);

        MVCubeModelBase targetModel = GetTargetModel();

        if (targetModel != null && targetModel.id != 75579) // root id
        {
            instance.StartCoroutine(BeginImport(targetModel).WrapToIl2Cpp());
            return;
        }

        state = ModelImporterState.WaitingForModel;
        RequestCubeModel(data.Scale);
    }

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

        if (data == null || data.Length == 0)
        {
            throw new Exception("Loaded model data is empty or invalid.");
        }

        return data;
    }

    private static void OnWORecieved(MVWorldObjectClient wo, int instigatorActorNr)
    {
        if (state == ModelImporterState.WaitingForModel && instigatorActorNr == MVGameControllerBase.Game.LocalPlayer.ActorNr)
        {
            if (TryGetModelFromWO(wo, out var model))
            {
                instance.StartCoroutine(BeginImport(model).WrapToIl2Cpp());
            }
        }
    }

    private static IEnumerator BeginImport(MVCubeModelBase model)
    {
        targetModelID = model.id;
        state = ModelImporterState.ImportInProgress;

        yield return instance.StartCoroutine(BuildModel(model, data).WrapToIl2Cpp());

        ResetState();

        NotifySuccess("Model imported successfully.");
    }

    [HarmonyPatch(typeof(MVWorldObjectClientManagerNetwork), "DestroyWO")]
    [HarmonyPrefix]
    private static void UnregisterWorldObject(int id)
    {
        if (state == ModelImporterState.ImportInProgress && id == targetModelID)
        {
            instance.StopAllCoroutines();
            ResetState();
            NotifySuccess("Model import was aborted.");
        }
    }
}