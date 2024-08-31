using HarmonyLib;
using KogamaTools.Helpers;
using UGUI.Desktop.Scripts.EditMode.Gizmo;

namespace KogamaTools.Features.Build;

[HarmonyPatch]
internal static class RotationStep
{
    internal static float Step = ConfigHelper.GetConfigValue<float>("RotationStep");
    internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("RotationStepEnabled");


    [HarmonyPatch(typeof(RotationHelper), "RotateStep")]
    [HarmonyPrefix]
    private static void ApplyRotation(ref float rotationSpeed)
    {
        if (Enabled)
        {
            rotationSpeed = Step * Math.Sign(rotationSpeed);
        }
    }
}
