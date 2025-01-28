using HarmonyLib;
using MV.WorldObject;

namespace KogamaTools.Tools.Build.UndoRedo.Actions;

[HarmonyPatch]
internal class PaintCubeAction : IUndoRedoAction
{
    public MVWorldObjectClient Target => target;

    private MVCubeModelBase target;
    private IntVector cubePos;
    private Cube oldCube;
    private Cube newCube;

    public PaintCubeAction(MVCubeModelBase target, IntVector cubePos, Cube oldCube, Cube newCube)
    {
        this.target = target;
        this.cubePos = cubePos;
        this.oldCube = oldCube;
        this.newCube = newCube;
    }

    public void Redo()
    {
        target.prototypeCubeModel.RemoveCube(cubePos);
        target.prototypeCubeModel.AddCube(cubePos, newCube);
        target.HandleDelta();
    }

    public void Undo()
    {
        target.prototypeCubeModel.RemoveCube(cubePos);
        target.prototypeCubeModel.AddCube(cubePos, oldCube);
        target.HandleDelta();
    }

    [HarmonyPatch(typeof(MVCubeModelBase), "ReplaceCube")]
    [HarmonyPrefix]
    private static void ReplaceCubePrefix(MVCubeModelBase __instance, IntVector iVector, ref Cube __state)
    {
        __state = __instance.GetCube(iVector).Clone();
    }

    [HarmonyPatch(typeof(MVCubeModelBase), "ReplaceCube")]
    [HarmonyPostfix]
    private static void ReplaceCubePostfix(MVCubeModelBase __instance, IntVector iVector, Cube __state)
    {
        UndoRedoManager.PushAction(new PaintCubeAction(__instance, iVector, __state, __instance.GetCube(iVector)));
    }
}
