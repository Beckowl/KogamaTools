using KogamaTools.Features.Build;
using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands;

internal class FastLinksCommand : BaseCommand
{
    public FastLinksCommand() : base("/fastlinks", "Disables link validation on the client side.")
    {
        AddVariant(args => Toggle());
    }

    private void Toggle()
    {
        FastLinks.Enabled = !FastLinks.Enabled;
        NotificationHelper.NotifySuccess($"Fast links {(FastLinks.Enabled ? "enabled" : "disabled")}.");
    }
}
