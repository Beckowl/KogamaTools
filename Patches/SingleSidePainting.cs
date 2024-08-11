using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Patches
{
    internal static class SingleSidePainting
    {
        internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("SingleSidePaintingEnabled");

        [HarmonyPatch(typeof(PaintCubes), "Execute")]
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

            bool picked = false;
            if (MVInputWrapper.GetBooleanControl(KogamaControls.PointerSelect) && e.SelectedCube != null)
            {
                CubePickingInfo pickingInfo = new CubePickingInfo();
                if (EditModeObjectPicker.GetPickingInfo(e.TargetCubeModel, ref pickingInfo))
                {
                    e.HandleAudio(e.SelectedCube.iLocalPos, AudioActions.CubeAdded);
                    e.TargetCubeModel.SetMaterial(e.SelectedCube.iLocalPos, pickingInfo.pickedFace, e.CurrentMaterialId);
                    CubeModelTool.SendCubeEvent(e.TargetCubeModel.CubeCount, EditCubeChange.CubePainted);
                }
                picked = true;
            }

            __instance.paintCursor.UpdateCursor(e.SelectedCube, e.TargetCubeModel, picked);
            return false;
        }
    }
}