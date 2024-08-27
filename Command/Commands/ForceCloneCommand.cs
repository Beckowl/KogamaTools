using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.Command.Commands;

internal class ForceCloneCommand : BaseCommand
{
    public ForceCloneCommand() : base("/forceclone", "")
    {
        AddVariant(args => Toggle());
    }

    private static void Toggle()
    {
        ForceFlags.ToggleFlags(InteractionFlags.CanClone);
        NotificationHelper.NotifySuccess($"Force clone {(ForceFlags.AreFlagsSet(InteractionFlags.CanClone) ? "enabled" : "disabled")}.");
    }
}
