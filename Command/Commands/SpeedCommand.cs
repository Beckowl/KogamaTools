using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/speed")]
[CommandDescription("Sets a speed multiplier for movement in edit mode.")]
internal class SpeedCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        EditModeSpeed.MultiplierEnabled = !EditModeSpeed.MultiplierEnabled;
        NotificationHelper.NotifySuccess($"Speed Multiplier {(EditModeSpeed.MultiplierEnabled ? "enabled" : "disabled")}.");
    }

    [CommandVariant]
    private void SetSpeed(float multiplier)
    {
        EditModeSpeed.Multiplier = multiplier;
        NotificationHelper.NotifySuccess($"Speed Multiplier set to {multiplier}.");
        EditModeSpeed.MultiplierEnabled = true;
    }
}
