using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.Command.Commands;

internal class RotationStepCommand : BaseCommand
{
    public RotationStepCommand() : base("/rotationstep", "Defines the rotation angle in degrees for when something is rotated.")
    {
        AddVariant(args => Toggle());
        AddVariant(args => SetSpeed((float)args[0]), typeof(float));
    }

    private void Toggle()
    {
        RotationStep.Enabled = !RotationStep.Enabled;
        NotificationHelper.NotifySuccess($"Custom rotation step {(RotationStep.Enabled ? "enabled" : "disabled")}.");
    }

    private void SetSpeed(float step)
    {
        RotationStep.Step = step;
        NotificationHelper.NotifySuccess($"Rotation step set to {step}.");
        RotationStep.Enabled = true;
    }
}
