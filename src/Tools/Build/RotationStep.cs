using HarmonyLib;
using KogamaTools.Config;
using UGUI.Desktop.Scripts.EditMode.Gizmo;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
[Section("Build")]
internal static class RotationStep
{
    [Bind] internal static bool Enabled = false;
    [Bind] internal static float Step = 15f;

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
