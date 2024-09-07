using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands;

#if DEBUG
[CommandName("/test")]
[CommandDescription("Prints a message to the console.")]
internal class TestCommand : BaseCommand
{
    [CommandVariant]
    private void TestMessage()
    {
        NotificationHelper.NotifyUser("Test command is working!! :)");
    }
}
#endif
