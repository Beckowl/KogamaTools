using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands;

[CommandName("/help")]
[CommandDescription("Lists all commands available.")]
internal class HelpCommand : BaseCommand
{
    [CommandVariant]
    private void DisplayList()
    {
        CommandHandler.ListCommands();
    }
}
