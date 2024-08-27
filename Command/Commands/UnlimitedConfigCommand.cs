using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.Command.Commands;

internal class UnlimitedConfigCommand : BaseCommand
{
    public UnlimitedConfigCommand() : base("/unlimitedconfig", "")
    {
        AddVariant(args => Toggle());
        AddVariant(args => SetValues((float)args[0], UnlimitedConfig.MaxValue), typeof(float));
        AddVariant(args => SetValues((float)args[0], (float)args[1]), typeof(float), typeof(float));
    }
    private void Toggle()
    {
        UnlimitedConfig.Enabled = !UnlimitedConfig.Enabled;
        NotificationHelper.NotifySuccess($"Unlimited config {(UnlimitedConfig.Enabled ? "Enabled" : "Disabled")}.");
    }
    private void SetValues(float minValue, float maxValue)
    {
        UnlimitedConfig.MinValue = minValue;
        UnlimitedConfig.MaxValue = maxValue;

        UnlimitedConfig.Enabled = true;

        NotificationHelper.NotifySuccess($"Unlimited config range set to {UnlimitedConfig.MinValue} - {UnlimitedConfig.MaxValue}.");
    }
}
