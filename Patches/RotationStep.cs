using HarmonyLib;
using KogamaTools.Helpers;
using UGUI.Desktop.Scripts.EditMode.Gizmo;

namespace KogamaTools.Patches
{
    internal static class RotationStep
    {
        internal static float Step = ConfigHelper.GetConfigValue<Single>("RotationStep");
        internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("RotationStepEnabled");

        [HarmonyPatch(typeof(RotationHelper), "RotateStep")]
        [HarmonyPrefix]
        private static void RotateStep(ref float rotationSpeed)
        {
            if (Enabled)
            {
                rotationSpeed = Step * Math.Sign(rotationSpeed);
            }
        }
    }
}
