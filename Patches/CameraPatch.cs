using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Patches
{
    internal static class CameraPatch
    {
        internal static bool BlueModeEnabled = ConfigHelper.GetConfigValue<bool>("BlueModeEnabled");
        internal static bool CustomFOVEnabled = ConfigHelper.GetConfigValue<bool>("CustomFOVEnabled");
        internal static float CustomFOV = ConfigHelper.GetConfigValue<float>("FOV");

        internal static bool CustomFOVSurpressed = false;

        [HarmonyPatch(typeof(MainCameraManager), "UpdateCamera")]
        [HarmonyPostfix]
        private static void UpdateCamera(MainCameraManager __instance)
        {
            __instance.BlueModeEnabled = __instance.BlueModeEnabled && BlueModeEnabled;
            if (CustomFOVEnabled && !CustomFOVSurpressed)
            {
                __instance.FieldOfView = CustomFOV;
            }
        }
    }
}

