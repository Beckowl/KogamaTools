using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.Command.Commands
{
    internal class CrossHairColorCommand : BaseCommand
    {
        public CrossHairColorCommand() : base("/crosshaircolor", "")
        {
            AddVariant(args => SetColor((string)args[0]), typeof(string));
            AddVariant(args => Toggle());
        }

        private void Toggle()
        {
            CustomCrossHairColor.CustomColorEnabled = !CustomCrossHairColor.CustomColorEnabled;
            NotificationHelper.NotifySuccess($"Crosshair color {(CustomCrossHairColor.CustomColorEnabled ? "enabled" : "disabled")}.");
        }

        private void SetColor(string colorStr)
        {
            if (CustomCrossHairColor.SetColorFromHTMLString(colorStr))
            {
                NotificationHelper.NotifySuccess($"Crosshair color set to \"{colorStr}\".");
                return;
            }
            NotificationHelper.WarnUser($"The color \"{colorStr}\" is invalid!");
        }
    }
}
