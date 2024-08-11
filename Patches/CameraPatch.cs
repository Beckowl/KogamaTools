using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(MainCameraManager))]
    internal static class CameraPatch
    {
        internal static bool BlueModeEnabled = ConfigHelper.GetConfigValue<bool>("BlueModeEnabled");

        [HarmonyPatch("UpdateCamera")]
        [HarmonyPostfix]
        private static void UpdateCamera(MainCameraManager __instance)
        {
            __instance.BlueModeEnabled = __instance.BlueModeEnabled && BlueModeEnabled;
            // TODO: Add "/fog" command;
            // RenderSettings.fog = false;
        }
    }
}

