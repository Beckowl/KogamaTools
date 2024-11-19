using BepInEx;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using KogamaTools.Helpers;
using KogamaTools.Tools.Misc;
using MV.WorldObject;
using RTG;
using UnityEngine;
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
    private static MVCubeModelBase targetModel = null!;
    private static ModelImporterState state = ModelImporterState.None;
    private static DeserializedModelData deserializedModelData = null!;
    private static string currentModelPath = null!;
    internal static ModelImporter instance = null!;

    private void Awake()
    {
        instance ??= this;
        WOReciever.OnWORecieved += OnWORecieved;
    }

    private static void ResetState()
    {
        targetModel = null!;
        state = ModelImporterState.None;
        deserializedModelData = null!;
        currentModelPath = null!;
    }

    public static bool LoadModelData(string path)
    {
        ResetState();
        NotificationHelper.NotifyUser("Loading model data...");
        try
        {
            if (!Path.IsPathFullyQualified(path))
            {
                path = Path.Combine(Paths.PluginPath, $"{KogamaTools.ModName}\\Models\\{path}");
            }

            if (!File.Exists(path))
            {
                NotificationHelper.WarnUser($"File does not exist: {path}");
                return false;
            }

            byte[] data = File.ReadAllBytes(path);
            currentModelPath = path;

            deserializedModelData = DeserializeModel(data);
            NotificationHelper.NotifySuccess("Model data loaded successfully.");

            RequestCubeModel(deserializedModelData.Scale);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static DeserializedModelData DeserializeModel(byte[] data)
    {
        try
        {
            using MemoryStream memoryStream = new(data);
            using BinaryReader reader = new(memoryStream);

            string signature = reader.ReadString();
            if (signature != "KTMODEL")
            {
                throw new Exception($"Invalid model file.");
            }

            float scale = reader.ReadSingle();
            Dictionary<IntVector, Cube> cubes = new();

            while (memoryStream.Position < memoryStream.Length)
            {
                short x = reader.ReadInt16();
                short y = reader.ReadInt16();
                short z = reader.ReadInt16();
                IntVector cubePos = new(x, y, z);

                byte[] faceMaterials = reader.ReadBytes(6);
                byte[] byteCorners = reader.ReadBytes(8);

                if (!MVMaterialRepository.instance.IsMaterialUnlocked(new Il2CppStructArray<byte>(faceMaterials)))
                {
                    NotificationHelper.WarnUser($"Replacing materials at {cubePos.ToString()}: Material is locked.");

                    faceMaterials = DefaultMaterials;
                }

                cubes.Add(cubePos, MakeCubeFromBytes(byteCorners, faceMaterials));
            }

            return new DeserializedModelData(scale, cubes);
        }
        catch (Exception ex)
        {
            NotificationHelper.NotifyError($"Failed to load model data: {ex.Message}");
            throw;
        }
    }

    private static void RequestCubeModel(float scale)
    {
        MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>().editorWorldObjectCreation.OnAddNewPrototype(string.Empty, scale);
        state = ModelImporterState.WaitingForModel;
    }

    private static void OnWORecieved(MVWorldObjectClient root, int instigatorActorNr)
    {
        if (state == ModelImporterState.WaitingForModel && instigatorActorNr == MVGameControllerBase.Game.LocalPlayer.ActorNr)
        {
            if (GetModelFromWO(root, out targetModel))
            {
                state = ModelImporterState.ImportInProgress;
                instance.StartCoroutine(BuildModel(targetModel, deserializedModelData.Cubes).WrapToIl2Cpp());
            }
        }
    }

    private static System.Collections.IEnumerator BuildModel(MVCubeModelBase model, Dictionary<IntVector, Cube> cubes)
    {
        NotificationHelper.NotifyUser("Importing model. You can delete the target model at any time to abort the process.");
        foreach (KeyValuePair<IntVector, Cube> kvp in cubes)
        {
            if (state != ModelImporterState.ImportInProgress)
            {
                yield break;
            }

            AddCubeToModel(kvp.Key, kvp.Value, model);
            yield return new WaitForSeconds(Mathf.Max(1f / 60f - Time.deltaTime, 0f));
        }

        NotificationHelper.NotifySuccess("Model import complete.");
        ResetState();
    }

    [HarmonyPatch(typeof(MVWorldObjectClient), "Delete")]
    [HarmonyPrefix]
    private static void UnregisterWorldObject(MVWorldObjectClient __instance)
    {
        if (state == ModelImporterState.ImportInProgress && __instance.id == targetModel.id)
        {
            ResetState();
        }
    }

    private class DeserializedModelData
    {
        public float Scale { get; }
        public Dictionary<IntVector, Cube> Cubes { get; }

        public DeserializedModelData(float scale, Dictionary<IntVector, Cube> cubes)
        {
            Scale = scale;
            Cubes = cubes;
        }
    }

}

