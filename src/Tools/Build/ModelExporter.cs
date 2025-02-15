﻿using KogamaTools.Behaviours;
using KogamaTools.Helpers;
using static System.IO.Path;
using static KogamaTools.Helpers.ModelHelper;

namespace KogamaTools.Tools.Build;
internal static class ModelExporter
{
    [InvokeOnInit]
    internal static void Init()
    {
        CustomContextMenu.AddButton("Export Model", wo => IsModelOwner(wo), wo => ExportModel(wo));
    }

    internal static void ExportModel(MVWorldObjectClient wo)
    {
        if (TryGetModelFromWO(wo, out var model))
        {
            ExportModel(model);
        }
    }

    internal static void ExportModel(MVCubeModelBase model)
    {
        ModelData data = GetModelData(model);
        byte[] serializedData = SerializeModelData(data);

        WriteDataToDisk(serializedData, model.id.ToString());
        NotificationHelper.NotifySuccess($"Use /importmodel to import a model into the game.");
    }

    private static void WriteDataToDisk(byte[] modelData, string filename)
    {
        try
        {
            string modelPath = Combine(ModelsPath, $"{filename}.ktm");
            string directoryPath = GetDirectoryName(modelPath)!;

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllBytes(modelPath, modelData);

            NotificationHelper.NotifySuccess($"Model exported to {modelPath}.");
        }
        catch (Exception ex)
        {
            KogamaTools.mls.LogError($"Failed to save model data: {ex.ToString()}");
        }
    }
}
