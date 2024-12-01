using HarmonyLib;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class SingleSidePainting
{
    internal static bool Enabled = false;

    [HarmonyPatch(typeof(PaintCubes), "Execute")]
    [HarmonyPrefix]
    static bool ReplaceCube(PaintCubes __instance, ref CubeModelingStateMachine e)
    {
        if (!Enabled) return true;

        if (__instance.waitForMouseUp)
        {
            __instance.waitForMouseUp = MVInputWrapper.GetBooleanControl(KogamaControls.PointerSelect);
            return false;
        }

        bool isPainting = false;

        if (MVInputWrapper.GetBooleanControl(KogamaControls.PointerSelect) && e.SelectedCube != null)
        {
            e.HandleAudio(e.SelectedCube.iLocalPos, AudioActions.CubeAdded);
            e.TargetCubeModel.SetMaterial(e.SelectedCube.iLocalPos, e.SelectedCube.pickedFace, e.CurrentMaterialId);
            CubeModelTool.SendCubeEvent(e.TargetCubeModel.CubeCount, EditCubeChange.CubePainted);

            isPainting = true;
        }
        __instance.paintCursor.UpdateCursor(e.SelectedCube, e.TargetCubeModel, isPainting);
        return false;
    }
}
