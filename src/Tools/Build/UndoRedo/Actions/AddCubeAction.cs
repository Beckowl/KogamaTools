using HarmonyLib;
using KogamaTools.Helpers;
using MV.WorldObject;

namespace KogamaTools.Tools.Build.UndoRedo.Actions;

[HarmonyPatch]
internal class AddCubeAction : IUndoRedoAction
{
    public MVWorldObjectClient Target => target;

    private MVCubeModelBase target;
    private IntVector cubePos;
    private Cube cube;

    public AddCubeAction(MVCubeModelBase target, IntVector cubePos, Cube cube)
    {
        this.target = target;
        this.cubePos = cubePos;
        this.cube = cube;
    }

    public void Redo()
    {
        target.prototypeCubeModel.AddCube(cubePos, cube);
        target.HandleDelta();
    }

    public void Undo()
    {
        target.prototypeCubeModel.RemoveCube(cubePos);
        target.HandleDelta();
    }

    [HarmonyPatch(typeof(MVCubeModelBase), "AddCube")]
    [HarmonyPrefix]
    private static bool AddCube(MVCubeModelBase __instance, IntVector pos, Cube cube)
    {
        if (ModelHelper.BuildInProgress) return true;

        __instance.MakeUnique();
        if (__instance.prototypeCubeModel.AddCube(pos, cube))
        {
            __instance.changedEventArgsQueue.Enqueue(new CubeModelChangedEventArgs(CubeAction.Added, pos, __instance));
            UndoRedoManager.PushAction(new AddCubeAction(__instance, pos, cube));
        }

        return false;
    }
}
