using HarmonyLib;
using UGUI.Desktop.Scripts.EditMode.Gizmo;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class RotationStep
{
    internal static float Step = 15f;
    internal static bool Enabled = false;


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
