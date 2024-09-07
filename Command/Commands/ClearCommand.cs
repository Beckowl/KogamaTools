using KogamaTools.Helpers;
namespace KogamaTools.Command.Commands;

[CommandName("/clear")]
[CommandDescription("Prints a bunch of newlines to pretend it's clearing the chat.")]
internal class ClearCommand : BaseCommand
{
    [CommandVariant]
    private void clearChat()
    {
        NotificationHelper.NotifyUser("\n\n\n\n\n\n\n\n\n\n");
    }
}
