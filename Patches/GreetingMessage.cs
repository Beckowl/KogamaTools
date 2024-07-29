using HarmonyLib;
using KogamaTools.Helpers;
namespace KogamaTools.patches
{
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests))]
    internal static class GreetingMessage
    {
        internal static bool ShowGreetingMessage = ConfigHelper.GetConfigValue<bool>("ShowGreetingMessage");

        [HarmonyPatch("JoinNotification")]
        [HarmonyPrefix]
        private static void JoinNotification()
        {
            TextCommand.NotifyUser($"Welcome to {KogamaTools.ModName} v{KogamaTools.ModVersion}.");
        }
    }
}
