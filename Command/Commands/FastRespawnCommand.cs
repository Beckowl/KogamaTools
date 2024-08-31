using KogamaTools.Helpers;
using KogamaTools.Tools.PVP;

namespace KogamaTools.Command.Commands;

internal class FastRespawnCommand : BaseCommand
{
    public FastRespawnCommand() : base("/fastrespawn", "")
    {
        AddVariant(args => Toggle());
    }

    private void Toggle()
    {
        FastRespawn.Enabled = !FastRespawn.Enabled;
        NotificationHelper.NotifySuccess($"Fast respawn {(FastRespawn.Enabled ? "enabled" : "disabled")}.");
    }
}
