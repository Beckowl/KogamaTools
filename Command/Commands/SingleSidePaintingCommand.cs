using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

internal class SingleSidePaintingCommand : BaseCommand
{
    public SingleSidePaintingCommand() : base("/singlesidepainting", "Enables individual painting of cube faces.")
    {
        AddVariant(args => Toggle());
    }

    private void Toggle()
    {
        SingleSidePainting.Enabled = !SingleSidePainting.Enabled;
        NotificationHelper.NotifySuccess($"Single side painting {(SingleSidePainting.Enabled ? "enabled" : "disabled")}.");
    }
}
