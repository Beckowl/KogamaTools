using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/bluemode")]
[CommandDescription("Disables the blue background in model editing.")]
internal class BlueModeCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        BlueModeController.BlueModeEnabled = !BlueModeController.BlueModeEnabled;
        NotificationHelper.NotifySuccess($"Blue mode {(BlueModeController.BlueModeEnabled ? "enabled" : "disabled")}.");
    }
}
