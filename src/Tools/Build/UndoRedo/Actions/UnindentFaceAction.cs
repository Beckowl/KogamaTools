using HarmonyLib;
using MV.WorldObject;

namespace KogamaTools.Tools.Build.UndoRedo.Actions;

[HarmonyPatch]
internal class UnindentFaceAction : IUndoRedoAction
{
    public MVWorldObjectClient Target => target;

    private MVCubeModelBase target;
    private IntVector cubePos;
    private Cube oldCube;
    private Cube newCube;

    public UnindentFaceAction(MVCubeModelBase target, IntVector cubePos, Cube oldCube, Cube newCube)
    {
        this.target = target;
        this.cubePos = cubePos;
        this.oldCube = oldCube;
        this.newCube = newCube;
    }

    public void Redo()
    {
        target.PrototypeCubeModel.RemoveCube(cubePos);
        target.PrototypeCubeModel.AddCube(cubePos, newCube);
        target.HandleDelta();
    }

    public void Undo()
    {
        target.PrototypeCubeModel.RemoveCube(cubePos);
        target.PrototypeCubeModel.AddCube(cubePos, oldCube);
        target.HandleDelta();
    }

    [HarmonyPatch(typeof(MVCubeModelBase), "UnIndentCubeFace")]
    [HarmonyPrefix]
    private static void UnIndentCubeFacePrefix(MVCubeModelBase __instance, IntVector localPos, Face face, Cube cube, ref Cube __state)
    {
        __state = __instance.GetCube(localPos);
    }

    [HarmonyPatch(typeof(MVCubeModelBase), "UnIndentCubeFace")]
    [HarmonyPrefix]
    private static void UnIndentCubeFacePostfix(MVCubeModelBase __instance, IntVector localPos, Face face, Cube cube, Cube __state)
    {
        UndoRedoManager.PushAction(new UnindentFaceAction(__instance, localPos, __state, cube));
    }
}
