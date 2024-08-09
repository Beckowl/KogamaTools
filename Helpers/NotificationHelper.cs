namespace KogamaTools.Helpers
{
    internal static class NotificationHelper
    {
        public static void NotifyUser(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                throw new ArgumentNullException(nameof(msg));

            TextCommand.NotifyUser(msg);
        }

        public static void NotifySuccess(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                throw new ArgumentNullException(nameof(msg));

            TextCommand.NotifyUser($"<color=cyan>{msg}</color>");
        }

        public static void WarnUser(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                throw new ArgumentNullException(nameof(msg));

            TextCommand.NotifyUser($"<color=yellow>{msg}</color>");
        }

        public static void NotifyError(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                throw new ArgumentNullException(nameof(msg));

            TextCommand.NotifyUser($"<color=red>{msg}</color>");
        }


    }
}
