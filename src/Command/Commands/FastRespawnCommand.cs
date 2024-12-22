using KogamaTools.Helpers;
using KogamaTools.Tools.PVP;

namespace KogamaTools.Command.Commands;

[CommandName("/fastrespawn")]
[CommandDescription("Skips the death animation when you press K.")]
internal class FastRespawnCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        FastRespawn.Enabled = !FastRespawn.Enabled;
        NotificationHelper.NotifySuccess($"Fast respawn {(FastRespawn.Enabled ? "enabled" : "disabled")}.");
    }
}
