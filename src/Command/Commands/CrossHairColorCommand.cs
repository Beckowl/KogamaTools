using KogamaTools.Helpers;
using KogamaTools.Tools.PVP;

namespace KogamaTools.Command.Commands;

[CommandName("/crosshaircolor")]
[CommandDescription("Sets a custom color for the crosshair.")]
internal class CrossHairColorCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        CustomCrossHairColor.Enabled = !CustomCrossHairColor.Enabled;
        NotificationHelper.NotifySuccess($"Crosshair color {(CustomCrossHairColor.Enabled ? "enabled" : "disabled")}.");
    }

    [CommandVariant]
    private void SetColor(string colorString)
    {
        if (ColorHelper.TryParseColorString(colorString, out CustomCrossHairColor.Color))
        {
            NotificationHelper.NotifySuccess($"Crosshair color set to \"{colorString}\".");
            CustomCrossHairColor.Enabled = true;
            return;
        }
        NotificationHelper.WarnUser($"The color \"{colorString}\" is invalid!");
    }
}
