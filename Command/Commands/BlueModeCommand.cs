using KogamaTools.patches;

namespace KogamaTools.Commands
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
            TextCommand.NotifyUser($"<color=cyan>Blue mode {(Camera.BlueModeEnabled ? "enabled" : "disabled")}.</color>");
        }
    }
}
