using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/linkfix")]
[CommandDescription("Allows connections between grouped logic.")]
internal class LinkFixCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        LinkFix.Enabled = !LinkFix.Enabled;
        NotificationHelper.NotifySuccess($"Linkfix {(LinkFix.Enabled ? "enabled" : "disabled")}.");
    }
}
