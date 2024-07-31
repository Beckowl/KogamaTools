using HarmonyLib;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(PaintCubes))]
    internal static class SingleSidePainting
    {
        internal static bool Enabled = false;

        [HarmonyPatch("Execute")]
        [HarmonyPrefix]
        static bool ReplaceCube(PaintCubes __instance, ref CubeModelingStateMachine e)
        {
            if (!Enabled)
            {
                return true;
            }

            if (__instance.waitForMouseUp)
            {
                __instance.waitForMouseUp = MVInputWrapper.GetBooleanControl(KogamaControls.PointerSelect);
                return false;
            }
            bool flag = false;
            if (MVInputWrapper.GetBooleanControl(KogamaControls.PointerSelect) && e.SelectedCube != null)
            {
                CubePickingInfo pickingInfo = new CubePickingInfo();
                if (EditModeObjectPicker.GetPickingInfo(e.TargetCubeModel, ref pickingInfo))
                {
                    e.HandleAudio(e.SelectedCube.iLocalPos, AudioActions.CubeAdded);
                    e.TargetCubeModel.SetMaterial(e.SelectedCube.iLocalPos, pickingInfo.pickedFace, e.CurrentMaterialId);
                    CubeModelTool.SendCubeEvent(e.TargetCubeModel.CubeCount, EditCubeChange.CubePainted);
                }
                flag = true;
            }
            __instance.paintCursor.UpdateCursor(e.SelectedCube, e.TargetCubeModel, flag);
            return false;
        }
    }
}
