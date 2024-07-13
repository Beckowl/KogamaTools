using KogamaTools.patches;

namespace KogamaTools.Command.Commands
{
    internal class FastLinksCommand : BaseCommand
    {
        public FastLinksCommand() : base("/fastlinks", "Disables link validation on the client side.")
        {
            AddVariant(args => Toggle());
        }

        private void Toggle()
        {
            FastLinks.Enabled = !FastLinks.Enabled;
            TextCommand.NotifyUser($"<color=cyan>Fast links {(FastLinks.Enabled ? "enabled" : "disabled")}.</color>");
        }
    }
}
