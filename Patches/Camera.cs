using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.patches
{
    [HarmonyPatch(typeof(MainCameraManager))]
    internal static class Camera
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

