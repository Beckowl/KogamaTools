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
        EditModeMovement.SpeedMultEnabled = !EditModeMovement.SpeedMultEnabled;
        NotificationHelper.NotifySuccess($"Speed Multiplier {(EditModeMovement.SpeedMultEnabled ? "enabled" : "disabled")}.");
    }

    [CommandVariant]
    private void SetSpeed(float multiplier)
    {
        EditModeMovement.SpeedMult = multiplier;
        NotificationHelper.NotifySuccess($"Speed Multiplier set to {multiplier}.");
        EditModeMovement.SpeedMultEnabled = true;
    }
}
