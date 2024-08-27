using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.Command.Commands;

internal class CustomScaleCommand : BaseCommand
{
    public CustomScaleCommand() : base("/modelscale", "Sets a custom scale for newly created models.")
    {
        AddVariant(args => Toggle());
        AddVariant(args => SetScale((float)args[0]), typeof(float));
    }

    private void Toggle()
    {
        CustomModelScale.Enabled = !CustomModelScale.Enabled;
        NotificationHelper.NotifySuccess($"Custom model scale {(CustomModelScale.Enabled ? "enabled" : "disabled")}");
    }

    private void SetScale(float scale)
    {
        if (scale == 0)
        {
            NotificationHelper.WarnUser("Model scale cannot be 0!");
            return;
        }
        CustomModelScale.CustomScale = scale;
        NotificationHelper.NotifySuccess($"Model scale set to {scale}.");
        CustomModelScale.Enabled = true;
    }
}
