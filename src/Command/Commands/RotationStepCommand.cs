using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/rotationstep")]
[CommandDescription("Defines the rotation angle in degrees for when something is rotated.")]
[BuildModeOnly]
internal class RotationStepCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        RotationStep.Enabled = !RotationStep.Enabled;
        NotificationHelper.NotifySuccess($"Custom rotation step {(RotationStep.Enabled ? "enabled" : "disabled")}.");
    }

    [CommandVariant]
    private void SetSpeed(float step)
    {
        RotationStep.Step = step;
        NotificationHelper.NotifySuccess($"Rotation step set to {step}.");
        RotationStep.Enabled = true;
    }
}
