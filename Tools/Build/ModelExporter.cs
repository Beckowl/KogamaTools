using BepInEx;
using KogamaTools.Helpers;
using MV.WorldObject;
using static KogamaTools.Helpers.ModelHelper;

namespace KogamaTools.Tools.Build;
internal static class ModelExporter
{
    internal static void Init()
    {
        CustomContextMenu.AddButton("Export Model", wo => IsModel(wo), wo => ExportModel(wo));
    }

    private static void ExportModel(MVWorldObjectClient wo)
    {
        if (GetModelFromWO(wo, out MVCubeModelBase model))
        {
            byte[] modelData = SerializeModel(model);
            try
            {
                string modelPath = Path.Combine(Paths.PluginPath, $"{KogamaTools.ModName}\\Models\\{model.id}.ktm");
                string directoryPath = Path.GetDirectoryName(modelPath)!;

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

    public static byte[] SerializeModel(MVCubeModelBase source)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        using (BinaryWriter writer = new BinaryWriter(memoryStream))
        {
            writer.Write("KTMODEL");
            writer.Write(source.prototypeCubeModel.Scale);

            foreach (CubeModelChunk chunk in source.prototypeCubeModel.Chunks.Values)
            {
                var enumerator = chunk.cells.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    IntVector cubePos = enumerator.Current.Key;
                    CubeBase cube = source.GetCube(cubePos);

                    writer.Write(cubePos.x);
                    writer.Write(cubePos.y);
                    writer.Write(cubePos.z);

                    writer.Write(cube.FaceMaterials);

                    writer.Write(cube.ByteCorners);
                }
            }

            return memoryStream.ToArray();
        }
    }
}
