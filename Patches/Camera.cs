using HarmonyLib;

namespace KogamaTools.patches
{
    [HarmonyPatch(typeof(MainCameraManager))]
    internal static class Camera
    {
        public static bool BlueModeEnabled = true;

        [HarmonyPatch("UpdateCamera")]
        [HarmonyPostfix]
        static void UpdateCamera(MainCameraManager __instance)
        {
            __instance.BlueModeEnabled = __instance.BlueModeEnabled && BlueModeEnabled;
        }
    }
}

