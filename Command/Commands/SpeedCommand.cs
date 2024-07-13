using KogamaTools.Patches;

namespace KogamaTools.Command.Commands
{
    internal class SpeedCommand : BaseCommand
    {
        public SpeedCommand() : base("/speed", "Sets a speed multiplier for movement in edit mode.")
        {
            AddVariant(args => Toggle());
            AddVariant(args => SetSpeed((float)args[0]), typeof(float));
        }

        private void Toggle()
        {
            EditModeMovement.speedMultEnabled = !EditModeMovement.speedMultEnabled;
            TextCommand.NotifyUser($"<color=cyan>Speed Multiplier {(EditModeMovement.speedMultEnabled ? "enabled" : "disabled")}.</color>");
        }

        private void SetSpeed(float speed)
        {
            EditModeMovement.speedMult = speed;
            TextCommand.NotifyUser($"<color=cyan>Speed Multiplier set to {speed}.</color>");
            EditModeMovement.speedMultEnabled = true;
        }
    }
}
