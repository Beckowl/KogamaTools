using HarmonyLib;
using MV.WorldObject;

namespace KogamaTools.Tools.Build.UndoRedo.Actions;

[HarmonyPatch]
internal class ChangeCornersAction : IUndoRedoAction
{

    public MVWorldObjectClient Target => target;

    private MVCubeModelBase target;
    private IntVector cubePos;
    private Cube oldCube;
    private Cube newCube;

    private static Cube oldCubeRecord;
    private static bool changeInProgress = false;

    public ChangeCornersAction(MVCubeModelBase target, IntVector cubePos, Cube oldCube, Cube newCube)
    {
        this.target = target;
        this.cubePos = cubePos;
        this.oldCube = oldCube;
        this.newCube = newCube;
    }

    public void Redo()
    {
        target.prototypeCubeModel.CornersChanged(cubePos, newCube);
        target.HandleDelta();
    }

    public void Undo()
    {
        target.prototypeCubeModel.CornersChanged(cubePos, oldCube);
        target.HandleDelta();
    }

    [HarmonyPatch(typeof(MVCubeModelBase), "CornersChanged")]
    [HarmonyPrefix]
    private static void CornersChanged(MVCubeModelBase __instance, IntVector iVector, Cube cube)
    {
        if (!changeInProgress)
        {
            oldCubeRecord = __instance.GetCube(iVector);
            changeInProgress = true;
        }
    }

    [HarmonyPatch(typeof(MVCubeModelBase), "CornersChangedDone")]
    [HarmonyPrefix]
    private static void CornersChangedDone(MVCubeModelBase __instance, IntVector iVector, Cube cube)
    {
        if (changeInProgress)
        {
            UndoRedoManager.PushAction(new ChangeCornersAction(__instance, iVector, oldCubeRecord, cube));
            changeInProgress = false;
        }
    }
}
