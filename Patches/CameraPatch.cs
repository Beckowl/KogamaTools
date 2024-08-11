using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(MainCameraManager))]
    internal static class CameraPatch
    {
        internal static bool BlueModeEnabled = ConfigHelper.GetConfigValue<bool>("BlueModeEnabled");
        internal static bool CustomFOVEnabled = false;
        internal static float CustomFOV = 60;

        [HarmonyPatch("UpdateCamera")]
        [HarmonyPostfix]
        private static void UpdateCamera(MainCameraManager __instance)
        {
            __instance.BlueModeEnabled = __instance.BlueModeEnabled && BlueModeEnabled;
            if (CustomFOVEnabled)
            {
                __instance.FieldOfView = CustomFOV;
            }

            // TODO: Add "/fog" command;
            // RenderSettings.fog = false;
        }
    }
}

