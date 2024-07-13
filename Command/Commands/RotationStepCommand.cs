using KogamaTools.Patches;

namespace KogamaTools.Command.Commands
{
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
            TextCommand.NotifyUser($"<color=cyan>Custom rotation step {(RotationStep.Enabled ? "enabled" : "disabled")}.</color>");
        }

        private void SetSpeed(float step)
        {
            RotationStep.Step = step;
            TextCommand.NotifyUser($"<color=cyan>Rotation step set to {step}.</color>");
            RotationStep.Enabled = true;
        }
    }
}
