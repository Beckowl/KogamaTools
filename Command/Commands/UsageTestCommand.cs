using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands;

#if DEBUG
internal class UsageTestCommand : BaseCommand
{
    public UsageTestCommand() : base("/usagetest", "Prints the first argument or \"Hello!\" if no argument is provided.")
    {
        AddVariant(args => NotificationHelper.NotifyUser("Hello!"));
        AddVariant(args => NotificationHelper.NotifyUser((string)args[0]), typeof(string))
            .SetUsage("/usagetest <message>");
    }
}
#endif
