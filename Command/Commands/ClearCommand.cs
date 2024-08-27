using KogamaTools.Helpers;
namespace KogamaTools.Command.Commands;

internal class ClearCommand : BaseCommand
{
    public ClearCommand() : base("/clear", "Prints a bunch of newlines to pretend it's clearing the chat")
    {
        AddVariant(args => NotificationHelper.NotifyUser(string.Concat(Enumerable.Repeat("\n", 10))));
    }
}
