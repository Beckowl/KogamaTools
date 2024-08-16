using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.Command.Commands
{
    internal class BlueModeCommand : BaseCommand
    {
        public BlueModeCommand() : base("/bluemode", "Disables the blue background in model editing.")
        {
            AddVariant(args => Toggle());
        }

        private void Toggle()
        {
            BlueMode.BlueModeEnabled = !BlueMode.BlueModeEnabled;
            NotificationHelper.NotifySuccess($"Blue mode {(BlueMode.BlueModeEnabled ? "enabled" : "disabled")}.");
        }
    }
}
