using System.Collections;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MV.WorldObject;
using UnityEngine;

namespace KogamaTools.Helpers;
internal static class ModelHelper
{
    private static readonly byte[] defaultMaterials = { 21, 21, 21, 21, 21, 21 };
    private static readonly string signature = "KTMODEL";

    internal static MVCubeModelBase GetTargetModel()
    {
        return RuntimeReferences.CubeModelingStateMachine.TargetCubeModel;
    }
    internal static bool TryGetModelFromWO(MVWorldObjectClient wo, out MVCubeModelBase modelBase)
    {
        modelBase = null!;

        if (IsModel(wo))
        {
            try
            {
                modelBase = wo.Cast<MVCubeModelBase>();
                return modelBase != null;
            }
            catch (Exception e)
            {
                NotificationHelper.NotifyError(e.ToString());
            }
        }

        return false;
    }
    internal static bool IsModelOwner(MVWorldObjectClient wo)
    {
        if (TryGetModelFromWO(wo, out MVCubeModelBase model))
        {
            return model.prototypeCubeModel.AuthorProfileID == MVGameControllerBase.Game.LocalPlayer.ProfileID;
        }
        return false;
    }

    internal static bool IsModel(MVWorldObjectClient wo)
    {
        return MVGameControllerBase.WOCM.IsType(wo.id, WorldObjectType.CubeModel);
    }

    internal static ModelData GetModelData(MVCubeModelBase model)
    {
        RuntimePrototypeCubeModel rpcm = model.prototypeCubeModel;

        float scale = rpcm.Scale;
        Dictionary<IntVector, Cube> cubes = new();

        foreach (CubeModelChunk chunk in rpcm.chunks.Values)
        {
            var enumerator = chunk.cells.GetEnumerator();

            while (enumerator.MoveNext())
            {
                IntVector cubePos = enumerator.Current.Key;
                Cube cube = rpcm.GetCube(cubePos);

                cubes.Add(cubePos, cube);
            }
        }

        return new ModelData(scale, cubes);
    }

    internal static byte[] SerializeModelData(ModelData data)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);

        writer.Write(signature);

        writer.Write(data.Scale);

        foreach (KeyValuePair<IntVector, Cube> kvp in data.Cubes)
        {
            IntVector cubePos = kvp.Key;
            Cube cube = kvp.Value;

            writer.Write(cubePos.x);
            writer.Write(cubePos.y);
            writer.Write(cubePos.z);

            writer.Write(cube.FaceMaterials);
            writer.Write(cube.byteCorners);
        }

        return memoryStream.ToArray();
    }

    internal static ModelData DeSerializeModelData(byte[] data)
    {
        float scale;
        Dictionary<IntVector, Cube> cubes = new();

        try
        {
            using MemoryStream memoryStream = new(data);
            using BinaryReader reader = new(memoryStream);

            string s = reader.ReadString();
            if (s != signature)
            {
                throw new Exception($"Invalid model data.");
            }

            scale = reader.ReadSingle();

            while (memoryStream.Position < memoryStream.Length)
            {
                short x = reader.ReadInt16();
                short y = reader.ReadInt16();
                short z = reader.ReadInt16();

                IntVector cubePos = new(x, y, z);

                byte[] faceMaterials = reader.ReadBytes(6);
                byte[] byteCorners = reader.ReadBytes(8);

                cubes.Add(cubePos, CubeFromBytes(byteCorners, faceMaterials));
            }

            return new ModelData(scale, cubes);
        }
        catch (Exception e)
        {
            NotificationHelper.WarnUser(e.Message);
            throw;
        }
    }

    internal static IEnumerator BuildModel(MVCubeModelBase target, ModelData data)
    {
        NotificationHelper.NotifyUser("The model build process has started. You can delete the target model at any time to abort it.");
        int PlacedCubes = 0;
        foreach (KeyValuePair<IntVector, Cube> kvp in data.Cubes)
        {
            IntVector cubePos = kvp.Key;
            Cube cube = kvp.Value;

            if (!MVMaterialRepository.instance.IsMaterialUnlocked(cube.faceMaterials))
            {
                NotificationHelper.WarnUser($"Replacing materials at {cubePos.ToString()}: Material is locked.");
                cube.faceMaterials = defaultMaterials;
            }

            AddCubeToModel(cubePos, cube, target);
            PlacedCubes++;

            if (PlacedCubes % 500 == 0)
            {
                yield return new WaitForSecondsRealtime(1f / 60f * 315);
            }    
        }
    }

    internal static void RequestCubeModel(float scale)
    {
        RuntimeReferences.EditorWorldObjectCreation.OnAddNewPrototype(string.Empty, scale);
    }

    private static Cube CubeFromBytes(byte[] byteCorners, byte[] faceMaterials)
    {
        var corners = new Il2CppStructArray<byte>(byteCorners);
        var faces = new Il2CppStructArray<byte>(faceMaterials);

        return new Cube(corners, faces);
    }

    private static void AddCubeToModel(IntVector position, Cube cube, MVCubeModelBase model)
    {
        if (model.ContainsCube(position))
        {
            model.RemoveCube(position);
        }

        model.AddCube(position, cube);
        model.HandleDelta();
    }

    internal class ModelData
    {
        public float Scale { get; }
        public Dictionary<IntVector, Cube> Cubes { get; }

        public ModelData(float scale, Dictionary<IntVector, Cube> cubes)
        {
            Scale = scale;
            Cubes = cubes;
        }
    }
}