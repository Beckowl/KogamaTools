using KogamaTools.Features.Build;
using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands;

internal class ForceRotation : BaseCommand
{
    public ForceRotation() : base("/forcerotation", "")
    {
        AddVariant(args => Toggle());
    }

    private static void Toggle()
    {
        InteractionFlags flags = InteractionFlags.CanRotateX | InteractionFlags.CanRotateY | InteractionFlags.CanRotateZ;
        ForceFlags.ToggleFlags(flags);

        bool isEnabled = ForceFlags.AreFlagsSet(InteractionFlags.CanRotateX | InteractionFlags.CanRotateY | InteractionFlags.CanRotateZ);
        NotificationHelper.NotifySuccess($"Unlocked rotation {(isEnabled ? "enabled" : "disabled")}.");
    }
}
