using HarmonyLib;
using KogamaTools.Config;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
[Section("Graphics")]
internal static class NotificationToggle
{
    [Bind] internal static bool ShowNotifications = true;

    [HarmonyPatch(typeof(NotificationController), "OnNotificationReceived")]
    [HarmonyPatch(typeof(NotificationsManager), "InstantiateNotification")]
    [HarmonyPrefix]
    private static bool OnNotificationRecieved()
    {
        return ShowNotifications;
    }
}
