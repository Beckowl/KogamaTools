using KogamaTools.patches;

namespace KogamaTools.Commands
{
    internal class CustomScaleCommand : CommandBase
    {
        public CustomScaleCommand() : base("/modelscale", "Sets a custom scale for newly created models.")
        {
            AddVariant(args => Toggle());
            AddVariant(args => SetScale((float)args[0]), typeof(float));
        }

        private void Toggle()
        {
            CustomModelScale.Enabled =! CustomModelScale.Enabled;
            TextCommand.NotifyUser($"<color=cyan>Custom model scale {(CustomModelScale.Enabled ? "enabled" : "disabled")}.</color>");
        }

        private void SetScale(float scale)
        {
            if (scale == 0)
            {
                TextCommand.NotifyUser($"<color=yellow>Model scale cannot be 0!</color>");
                return;
            }
            CustomModelScale.CustomScale = scale;
            TextCommand.NotifyUser($"<color=cyan>Model scale set to {scale}.</color>");
            CustomModelScale.Enabled = true;
        }
    }
}
