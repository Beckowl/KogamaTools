﻿using KogamaTools.Behaviours;
using KogamaTools.Config;
using KogamaTools.Helpers;

namespace KogamaTools.Tools.Misc;

internal static class GreetingMessage
{
    [Bind] internal static bool ShowGreetingMessage = true;

    [InvokeOnInit(priority: 2)]
    internal static void JoinNotification()
    {
        if (ShowGreetingMessage)
        {
            NotificationHelper.NotifySuccess($"Welcome to {KogamaTools.ModName} v{KogamaTools.ModVersion}.");
            NotificationHelper.NotifyUser("Press <b>F1</b> to toggle the overlay.\nType <b>/help</b> to view available chat commands.\nType <b>/hotkeys</b> to view the available hotkeys.");
#if DEBUG
            NotificationHelper.WarnUser("<b>This is a debug build intended for testing purposes only. It may be unstable and potentially insecure.</b>");
#endif
        }
    }
}
