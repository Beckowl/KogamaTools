using HarmonyLib;

namespace KogamaTools.Tools.Misc;

[HarmonyPatch]
internal static class NoCheatHandling
{

    [HarmonyPatch(typeof(CheatHandling), "Init")]
    [HarmonyPrefix]
    static bool Init()
    {
        return false;
    }

    [HarmonyPatch(typeof(CheatHandling), "MachineBanDetected")]
    [HarmonyPrefix]
    static bool MachineBanDetected()
    {
        return false;
    }

    [HarmonyPatch(typeof(CheatHandling), "ExecuteBan")]
    [HarmonyPrefix]
    static bool ExecuteBan()
    {
        return false;
    }

}

