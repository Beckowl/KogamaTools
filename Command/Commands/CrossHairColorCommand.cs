using KogamaTools.Features.PVP;
using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands;

internal class CrossHairColorCommand : BaseCommand
{
    public CrossHairColorCommand() : base("/crosshaircolor", "")
    {
        AddVariant(args => SetColor((string)args[0]), typeof(string));
        AddVariant(args => Toggle());
    }

    private void Toggle()
    {
        CrossHairMod.CustomCrossHairColorEnabled = !CrossHairMod.CustomCrossHairColorEnabled;
        NotificationHelper.NotifySuccess($"Crosshair color {(CrossHairMod.CustomCrossHairColorEnabled ? "enabled" : "disabled")}.");
    }

    private void SetColor(string colorStr)
    {
        if (ColorHelper.TryParseColorString(colorStr, out CrossHairMod.CrossHairColor))
        {
            NotificationHelper.NotifySuccess($"Crosshair color set to \"{colorStr}\".");
            return;
        }
        NotificationHelper.WarnUser($"The color \"{colorStr}\" is invalid!");
    }
}
