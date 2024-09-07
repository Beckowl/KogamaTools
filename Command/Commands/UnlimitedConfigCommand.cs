using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/unlimitedconfig")]
[CommandDescription("Defines a custom range for input fields/sliders.")]
internal class UnlimitedConfigCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        UnlimitedConfig.Enabled = !UnlimitedConfig.Enabled;
        NotificationHelper.NotifySuccess($"Unlimited config {(UnlimitedConfig.Enabled ? "enabled" : "disabled")}.");
    }

    [CommandVariant]
    private void SetRange(float minValue, float maxValue)
    {
        UnlimitedConfig.MinValue = minValue;
        UnlimitedConfig.MaxValue = maxValue;
        UnlimitedConfig.Enabled = true;

        NotificationHelper.NotifySuccess($"Unlimited config range set to {UnlimitedConfig.MinValue} - {UnlimitedConfig.MaxValue}.");
    }
}
