using HarmonyLib;
using MV.Common;

namespace KogamaTools.Tools.Misc;

[HarmonyPatch]
internal static class AntiBan
{
    [HarmonyPatch(typeof(CheatHandling), "Init")]
    [HarmonyPatch(typeof(CheatHandling), "ExecuteBan")]
    [HarmonyPatch(typeof(CheatHandling), "MachineBanDetected")]
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "Ban", new Type[] { typeof(int), typeof(MVPlayer), typeof(string) })]
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "Ban", new Type[] { typeof(CheatType) })]
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "Expel")]
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "Kick")]
    [HarmonyPrefix]
    private static bool AntiBanPatches()
    {
        return false;
    }
}

