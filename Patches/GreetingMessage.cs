﻿using HarmonyLib;
using KogamaTools.Helpers;
namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests))]
    internal static class GreetingMessage
    {
        internal static bool ShowGreetingMessage = ConfigHelper.GetConfigValue<bool>("ShowGreetingMessage");

        [HarmonyPatch("JoinNotification")]
        [HarmonyPostfix]
        private static void JoinNotification()
        {
            if (ShowGreetingMessage)
            {
                TextCommand.NotifyUser($"Welcome to {KogamaTools.ModName} v{KogamaTools.ModVersion}.");
            }
        }
    }
}
