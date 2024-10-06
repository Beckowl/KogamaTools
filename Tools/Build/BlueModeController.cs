using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class BlueModeController
{
    internal static bool BlueModeEnabled = true;

    [HarmonyPatch(typeof(MainCameraManager), "UpdateCamera")]
    [HarmonyPostfix]
    private static void UpdateCamera(MainCameraManager __instance)
    {
        __instance.BlueModeEnabled = __instance.BlueModeEnabled && BlueModeEnabled;
    }
}

