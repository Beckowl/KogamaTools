using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/modelscale")]
[CommandDescription("Sets a custom scale for newly created models.")]
[BuildModeOnly]
internal class ModelScaleCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        CustomModelScale.Enabled = !CustomModelScale.Enabled;
        NotificationHelper.NotifySuccess($"Custom model scale {(CustomModelScale.Enabled ? "enabled" : "disabled")}");
    }

    [CommandVariant]
    private void SetScale(float scale)
    {
        if (scale == 0)
        {
            NotificationHelper.WarnUser("Model scale cannot be 0!");
            return;
        }
        CustomModelScale.Scale = scale;
        NotificationHelper.NotifySuccess($"Model scale set to {scale}.");
        CustomModelScale.Enabled = true;
    }
}
