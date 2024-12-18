using HarmonyLib;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
internal class NotificationToggle
{
    internal static bool ShowNotifications = true;

    [HarmonyPatch(typeof(NotificationController), "OnNotificationReceived")]
    [HarmonyPatch(typeof(NotificationsManager), "InstantiateNotification")]
    [HarmonyPrefix]
    private static bool OnNotificationRecieved()
    {
        return ShowNotifications;
    }
}
