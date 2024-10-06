using HarmonyLib;
using KogamaTools.Behaviours;
using KogamaTools.Helpers;

namespace KogamaTools.Tools.Misc;

[HarmonyPatch]
internal static class GreetingMessage
{
    internal static bool ShowGreetingMessage = ConfigHelper.GetConfigValue<bool>("ShowGreetingMessage");

    internal static void JoinNotification()
    {
        if (ShowGreetingMessage)
        {
            NotificationHelper.NotifySuccess($"Welcome to {KogamaTools.ModName} v{KogamaTools.ModVersion}.");
            NotificationHelper.NotifyUser("Press F1 to toggle the overlay.\nType /help to see available chat commands.\n");
        }
    }
}
