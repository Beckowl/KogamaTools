using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Features.Build;

[HarmonyPatch]
internal static class BlueModeController
{
    internal static bool BlueModeEnabled = ConfigHelper.GetConfigValue<bool>("BlueModeEnabled");

    [HarmonyPatch(typeof(MainCameraManager), "UpdateCamera")]
    [HarmonyPostfix]
    private static void UpdateCamera(MainCameraManager __instance)
    {
        __instance.BlueModeEnabled = __instance.BlueModeEnabled && BlueModeEnabled;
    }
}

