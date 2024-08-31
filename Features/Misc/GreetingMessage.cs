using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Features.Misc;

[HarmonyPatch]
internal static class GreetingMessage
{
    internal static bool ShowGreetingMessage = ConfigHelper.GetConfigValue<bool>("ShowGreetingMessage");

    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "JoinNotification")]
    [HarmonyPostfix]
    private static void JoinNotification()
    {
        if (ShowGreetingMessage)
        {
            TextCommand.NotifyUser($"Welcome to {KogamaTools.ModName} v{KogamaTools.ModVersion}.");
        }
    }
}
