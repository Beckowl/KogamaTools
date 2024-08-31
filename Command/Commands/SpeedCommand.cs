using KogamaTools.Features.Build;
using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands;

internal class SpeedCommand : BaseCommand
{
    public SpeedCommand() : base("/speed", "Sets a speed multiplier for movement in edit mode.")
    {
        AddVariant(args => Toggle());
        AddVariant(args => SetSpeed((float)args[0]), typeof(float));
    }

    private void Toggle()
    {
        EditModeMovement.SpeedMultEnabled = !EditModeMovement.SpeedMultEnabled;
        NotificationHelper.NotifySuccess($"Speed Multiplier {(EditModeMovement.SpeedMultEnabled ? "enabled" : "disabled")}.");
    }

    private void SetSpeed(float speed)
    {
        EditModeMovement.SpeedMult = speed;
        NotificationHelper.NotifySuccess($"Speed Multiplier set to {speed}.");
        EditModeMovement.SpeedMultEnabled = true;
    }
}
