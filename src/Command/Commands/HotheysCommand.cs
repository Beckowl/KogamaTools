using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands;

// i wanted to add this to the info menu but i'm too lazy to do it right now

[CommandName("/hotkeys")]
[CommandDescription("Lists the hotkeys/shortcuts available.")]
internal class HotheysCommand : BaseCommand
{
    [CommandVariant]
    private void ShowHotkeys()
    {
        NotificationHelper.NotifyUser("• Press <b>F1</b> to toggle the overlay.\n• Press <b>F2</b> to take a screenshot.\n• Hold <b>Ctrl</b> and <b>left click</b> objects to multi-select.\n• Press <b>Ctrl + G</b> when multi-selecting to group the selected objects.");
    }
}
