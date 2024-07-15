using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.patches
{
    [HarmonyPatch(typeof(MainCameraManager))]
    internal static class Camera
    {
        public static bool BlueModeEnabled = ConfigHelper.GetConfigValue<bool>("BlueModeEnabled");

        [HarmonyPatch("UpdateCamera")]
        [HarmonyPostfix]
        static void UpdateCamera(MainCameraManager __instance)
        {
            __instance.BlueModeEnabled = __instance.BlueModeEnabled && BlueModeEnabled;
            RenderSettings.fog = false;
        }
    }
}

