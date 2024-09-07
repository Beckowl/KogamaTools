using KogamaTools.Helpers;
using KogamaTools.Tools.PVP;

namespace KogamaTools.Command.Commands;

[CommandName("/crosshaircolor")]
[CommandDescription("Sets a custom ")]
internal class CrossHairColorCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        CrossHairMod.CustomCrossHairColorEnabled = !CrossHairMod.CustomCrossHairColorEnabled;
        NotificationHelper.NotifySuccess($"Crosshair color {(CrossHairMod.CustomCrossHairColorEnabled ? "enabled" : "disabled")}.");
    }

    [CommandVariant]
    private void SetColor(string colorString)
    {
        if (ColorHelper.TryParseColorString(colorString, out CrossHairMod.CrossHairColor))
        {
            NotificationHelper.NotifySuccess($"Crosshair color set to \"{colorString}\".");
            return;
        }
        NotificationHelper.WarnUser($"The color \"{colorString}\" is invalid!");
    }
}
