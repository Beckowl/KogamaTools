using KogamaTools.Helpers;
using KogamaTools.patches;

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
            Camera.BlueModeEnabled = !Camera.BlueModeEnabled;
            NotificationHelper.NotifySuccess($"Blue mode {(Camera.BlueModeEnabled ? "enabled" : "disabled")}.");
        }
    }
}
