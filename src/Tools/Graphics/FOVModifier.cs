using HarmonyLib;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
internal static class FOVModifier
{
    internal static bool CustomFOVEnabled = false;
    internal static float CustomFOV = 60;
    internal static bool ApplyGlobally = false;

    [HarmonyPatch(typeof(MainCameraManager), "UpdateCamera")]
    [HarmonyPostfix]
    private static void UpdateCamera(MainCameraManager __instance)
    {
        if (ApplyGlobally)
        {
            __instance.FieldOfView = CustomFOV;
        }
    }
}