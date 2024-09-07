using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/fastlinks")]
[CommandDescription("Disables link validation on the client side.")]
internal class FastLinksCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        FastLinks.Enabled = !FastLinks.Enabled;
        NotificationHelper.NotifySuccess($"Fast links {(FastLinks.Enabled ? "enabled" : "disabled")}.");
    }
}
