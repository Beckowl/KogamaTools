using HarmonyLib;
namespace KogamaTools.patches
{
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests))]
    internal class GreetingMsg
    {
        [HarmonyPatch("JoinNotification")]
        [HarmonyPrefix]
        static void JoinNotification()
        {
            TextCommand.NotifyUser($"Welcome to {KogamaTools.ModName} v{KogamaTools.ModVersion}.");
        }
    }
}
