using System;
using HarmonyLib;
using UGUI.Desktop.Scripts.EditMode.Gizmo;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(RotationHelper))]
    internal static class RotationStep
    {
        public static float Step = 15f;
        public static bool Enabled = false;

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
