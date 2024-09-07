using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/custompaint")]
[CommandName("/singlesidepainting")]
[CommandDescription("Enables individual painting of cube faces.")]
internal class CustomPaintCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        SingleSidePainting.Enabled = !SingleSidePainting.Enabled;
        NotificationHelper.NotifySuccess($"Single side painting {(SingleSidePainting.Enabled ? "enabled" : "disabled")}.");
    }
}
