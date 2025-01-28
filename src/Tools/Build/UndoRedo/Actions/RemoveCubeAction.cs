using HarmonyLib;
using MV.WorldObject;

namespace KogamaTools.Tools.Build.UndoRedo.Actions;

[HarmonyPatch]
internal class RemoveCubeAction : IUndoRedoAction
{
    public MVWorldObjectClient Target => target;

    private MVCubeModelBase target;
    private IntVector cubePos;
    private Cube cube;

    public RemoveCubeAction(MVCubeModelBase target, IntVector cubePos, Cube cube)
    {
        this.target = target;
        this.cubePos = cubePos;
        this.cube = cube;
    }

    public void Redo()
    {
        target.prototypeCubeModel.RemoveCube(cubePos);
        target.HandleDelta();
    }

    public void Undo()
    {
        target.prototypeCubeModel.AddCube(cubePos, cube);
        target.HandleDelta();
    }

    [HarmonyPatch(typeof(MVCubeModelBase), "RemoveCube")]
    [HarmonyPrefix]
    private static void RemoveCube(MVCubeModelBase __instance, IntVector pos)
    {
        UndoRedoManager.PushAction(new RemoveCubeAction(__instance, pos, __instance.GetCube(pos)));
    }
}
