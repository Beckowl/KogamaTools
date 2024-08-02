using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.Command.Commands
{
    internal class ForceCloneCommand : BaseCommand
    {
        public ForceCloneCommand() : base("/forceclone", "")
        {
            AddVariant(args => Toggle());
        }

        private static void Toggle()
        {
            ForceFlags.Flags = ForceFlags.Flags ^ (ulong)InteractionFlags.CanClone;
            NotificationHelper.NotifySuccess($"Force clone {(ForceFlags.IsFlagSet(InteractionFlags.CanClone) ? "enabled" : "disabled")}.");
        }
    }
}
