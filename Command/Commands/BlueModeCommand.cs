using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

internal class BlueModeCommand : BaseCommand
{
    public BlueModeCommand() : base("/bluemode", "Disables the blue background in model editing.")
    {
        AddVariant(args => Toggle());
    }

    private void Toggle()
    {
        BlueModeController.BlueModeEnabled = !BlueModeController.BlueModeEnabled;
        NotificationHelper.NotifySuccess($"Blue mode {(BlueModeController.BlueModeEnabled ? "enabled" : "disabled")}.");
    }
}
