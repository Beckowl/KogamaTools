using System.Collections;
using Assets.Scripts.WorldObjectTypes.EditablePickupItem;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MV.WorldObject;
using UnityEngine;
using WorldObjectTypes.MVDoor;
using WorldObjectTypes.VehicleEnergy;
using static System.Environment;

namespace KogamaTools.Helpers;
internal static class ModelHelper
{
    internal static readonly string ModelsPath = Path.Combine(GetFolderPath(SpecialFolder.ApplicationData), KogamaTools.ModName, "Models");
    private static readonly byte[] defaultMaterials = { 21, 21, 21, 21, 21, 21 };
    private static readonly string signature = "KTMODEL";

    internal static MVCubeModelBase GetModelFromWorldObject(MVWorldObjectClient mVWorldObjectClient)
    {
        // MauryDev

        MVCubeModelBase result = null!;

        var mVWorldObjectClient_type = mVWorldObjectClient.GetIl2CppType();
        if (mVWorldObjectClient.Is<MVWorldObjectSpawnerVehicle>())
        {
            MVWorldObjectSpawnerVehicle spawnerVehicle = mVWorldObjectClient.Cast<MVWorldObjectSpawnerVehicle>();
            MVWorldObjectClient spawnWorldObject = MVGameControllerBase.WOCM.GetWorldObjectClient(spawnerVehicle.SpawnWorldObjectID);
            result = GetModelFromWorldObject(spawnWorldObject);
        }
        else if (mVWorldObjectClient.Is<MVSentryGunBlueprint>())
        {
            var sentryGunBlueprint = mVWorldObjectClient.Cast<MVSentryGunBlueprint>();
            result = sentryGunBlueprint.EditableCubesWO;
        }
        else if (mVWorldObjectClient.Is<MVMovingPlatformGroup>())
        {
            var movingplatform = mVWorldObjectClient.Cast<MVMovingPlatformGroup>();
            result = movingplatform.Platform.CubeModel;
        }
        else if (mVWorldObjectClient.Is<MVRotator>())
        {
            var rotator = mVWorldObjectClient.Cast<MVRotator>();

            result = rotator.CubeModel;
        }
        else if (mVWorldObjectClient.Is<CollectTheItemCollectable>())
        {
            var collectTheItemCollectable = mVWorldObjectClient.Cast<CollectTheItemCollectable>();
            result = collectTheItemCollectable.editableCubeModelWrapper.CubeModel;
        }
        else if (mVWorldObjectClient.Is<CollectTheItemDropOff>())
        {
            var collectTheItemDropOff = mVWorldObjectClient.Cast<CollectTheItemDropOff>();
            result = collectTheItemDropOff.editableCubeModelWrapper.CubeModel;
        }
        else if (mVWorldObjectClient.Is<MVAdvancedGhost>())
        {
            var advancedghost = mVWorldObjectClient.Cast<MVAdvancedGhost>();
            result = advancedghost.editableCubeModelWrapper.CubeModel;
        }
        else if (mVWorldObjectClient.Is<MVJetPack>())
        {
            var jetpack = mVWorldObjectClient.Cast<MVJetPack>();
            result = jetpack.editableCubeModelWrapper.CubeModel;
        }
        else if (mVWorldObjectClient.Is<MVHoverCraft>())
        {
            var hovercraft = mVWorldObjectClient.Cast<MVHoverCraft>();
            result = hovercraft.editableCubeModelWrapper.CubeModel;
        }
        else if (typeof(MVEditablePickupItemBaseBlueprint).GetIl2Type().IsAssignableFrom(mVWorldObjectClient_type))
        {
            var value = mVWorldObjectClient.Cast<MVEditablePickupItemBaseBlueprint>();

            result = value.editableCubeModel;
        }
        else if (mVWorldObjectClient.Is<MVWorldObjectSpawnerVehicleEnergy>())
        {
            var energy = mVWorldObjectClient.Cast<MVWorldObjectSpawnerVehicleEnergy>();
            result = energy.vehicleEnergyChild.CubeModelInstance;
        }
        else if (mVWorldObjectClient.Is<MVDoorBlueprint>())
        {
            var door = mVWorldObjectClient.Cast<MVDoorBlueprint>();
            result = door.DoorLogic.doorModelInstance;
        }
        else if (mVWorldObjectClient.Is<MVCubeModelInstance>())
        {
            result = mVWorldObjectClient.Cast<MVCubeModelBase>();
        }

        return result;
    }

    internal static MVCubeModelBase GetTargetModel()
    {
        return RuntimeReferences.CubeModelingStateMachine.TargetCubeModel;
    }
    internal static bool TryGetModelFromWO(MVWorldObjectClient wo, out MVCubeModelBase modelBase)
    {
        modelBase = GetModelFromWorldObject(wo);
        return modelBase != null;
    }
    internal static bool IsModelOwner(MVWorldObjectClient wo)
    {
        if (TryGetModelFromWO(wo, out MVCubeModelBase model))
        {
            var authorID = model.PrototypeCubeModel.AuthorProfileID;
            var profileID = MVGameControllerBase.LocalPlayer.ProfileID;

            return authorID == -1 || authorID == MVGameControllerBase.LocalPlayer.ProfileID;
        }
        return false;
    }

    internal static bool IsModel(MVWorldObjectClient wo)
    {
        return GetModelFromWorldObject(wo) != null;
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
        using MemoryStream memoryStream = new();
        using BinaryWriter writer = new(memoryStream);

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