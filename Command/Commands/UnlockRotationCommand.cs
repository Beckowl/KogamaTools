using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.Command.Commands
{
    internal class UnlockRotationCommand : BaseCommand
    {
        public UnlockRotationCommand() : base("/unlockrotation", "")
        {
            AddVariant(args => Toggle());
        }

        private static void Toggle()
        {
            ForceFlags.Flags = ForceFlags.Flags ^ (ulong)InteractionFlags.CanRotateX;
            ForceFlags.Flags = ForceFlags.Flags ^ (ulong)InteractionFlags.CanRotateY;
            ForceFlags.Flags = ForceFlags.Flags ^ (ulong)InteractionFlags.CanRotateZ;

            bool isEnabled = ForceFlags.IsFlagSet(InteractionFlags.CanRotateX | InteractionFlags.CanRotateY | InteractionFlags.CanRotateZ);
            NotificationHelper.NotifySuccess($"Unlocked rotation {(isEnabled ? "enabled" : "disabled")}.");
        }
    }
}
