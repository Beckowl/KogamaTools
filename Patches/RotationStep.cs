using System;
using HarmonyLib;
using KogamaTools.Helpers;
using UGUI.Desktop.Scripts.EditMode.Gizmo;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(RotationHelper))]
    internal static class RotationStep
    {
        public static float Step = ConfigHelper.GetConfigValue<Single>("RotationStep");
        public static bool Enabled = ConfigHelper.GetConfigValue<bool>("RotationStepEnabled");

        [HarmonyPatch("RotateStep")]
        [HarmonyPrefix]
        static void ApplyRotation(ref float rotationSpeed)
        {
            if (Enabled)
            {
                rotationSpeed = Step * Math.Sign(rotationSpeed);
            }
        }
    }
}
