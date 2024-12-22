using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands;
#if DEBUG
[CommandName("/echo")]
internal class EchoCommand : BaseCommand
{
    [CommandVariant]
    private void Echo(string message)
    {
        NotificationHelper.NotifyUser(message);
    }
}
#endif
