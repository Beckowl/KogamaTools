using HarmonyLib;
using KogamaTools.Config;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
[Section("Graphics")]
internal static class FOVModifier
{
    [Bind] internal static bool CustomFOVEnabled = false;
    [Bind] internal static float CustomFOV = 60;
    [Bind] internal static bool ApplyGlobally = false;

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