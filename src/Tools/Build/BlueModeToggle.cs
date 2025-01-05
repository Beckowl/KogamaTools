using HarmonyLib;
using KogamaTools.Config;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
[Section("Build")]
internal static class BlueModeToggle
{
    [Bind] internal static bool BlueModeEnabled = true;

    [HarmonyPatch(typeof(MainCameraManager), "UpdateCamera")]
    [HarmonyPostfix]
    private static void UpdateCamera(MainCameraManager __instance)
    {
        __instance.BlueModeEnabled = __instance.BlueModeEnabled && BlueModeEnabled;
    }
}

