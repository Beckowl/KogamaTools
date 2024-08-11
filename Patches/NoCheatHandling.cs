using HarmonyLib;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(CheatHandling))]
    internal static class NoCheatHandling
    {

        [HarmonyPatch("Init")]
        [HarmonyPrefix]
        static bool Init()
        {
            return false;
        }

        [HarmonyPatch("MachineBanDetected")]
        [HarmonyPrefix]
        static bool MachineBanDetected()
        {
            return false;
        }

        [HarmonyPatch("ExecuteBan")]
        [HarmonyPrefix]
        static bool ExecuteBan()
        {
            return false;
        }

    }
}