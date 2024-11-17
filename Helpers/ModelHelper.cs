using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MV.WorldObject;

namespace KogamaTools.Helpers;
internal static class ModelHelper
{
    internal static bool GetModelFromWO(MVWorldObjectClient wo, out MVCubeModelBase modelBase)
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
        if (GetModelFromWO(wo, out MVCubeModelBase model))
        {
            return model.prototypeCubeModel.AuthorProfileID == MVGameControllerBase.Game.LocalPlayer.ProfileID;
        }
        return false;
    }

    internal static bool IsModel(MVWorldObjectClient wo)
    {
        return MVGameControllerBase.WOCM.IsType(wo.id, WorldObjectType.CubeModel);
    }

    internal static Cube MakeCubeFromBytes(byte[] byteCorners, byte[] faceMaterials)
    {
        var corners = new Il2CppStructArray<byte>(byteCorners);
        var faces = new Il2CppStructArray<byte>(faceMaterials);

        return new Cube(corners, faces);
    }

    internal static void AddCubeToModel(IntVector position, Cube cube, MVCubeModelBase model)
    {
        if (model.ContainsCube(position))
        {
            model.RemoveCube(position);
        }

        model.AddCube(position, cube);
        model.HandleDelta();
    }

}
