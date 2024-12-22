namespace KogamaTools.Helpers;

internal static class NotificationHelper
{
    internal static void NotifyUser(string msg)
    {
        if (string.IsNullOrEmpty(msg))
            throw new ArgumentNullException(nameof(msg));

        TextCommand.NotifyUser(msg);
    }

    internal static void NotifySuccess(string msg)
    {
        if (string.IsNullOrEmpty(msg))
            throw new ArgumentNullException(nameof(msg));

        TextCommand.NotifyUser($"<color=cyan>{msg}</color>");
    }

    internal static void WarnUser(string msg)
    {
        if (string.IsNullOrEmpty(msg))
            throw new ArgumentNullException(nameof(msg));

        TextCommand.NotifyUser($"<color=yellow>{msg}</color>");
    }

    internal static void NotifyError(string msg)
    {
        if (string.IsNullOrEmpty(msg))
            throw new ArgumentNullException(nameof(msg));

        TextCommand.NotifyUser($"<color=red>{msg}</color>");
    }


}
