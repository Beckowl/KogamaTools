using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/nolimit")]
[CommandDescription("Disables the minumum cube count and modeling constraint of models/avatars.")]
[BuildModeOnly]
internal class NoLimitCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        NoLimit.Enabled = !NoLimit.Enabled;
        NotificationHelper.NotifySuccess($"No limit {(NoLimit.Enabled ? "enabled" : "disabled")}.");
    }
}
