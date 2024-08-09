using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.Command.Commands
{
    internal class CustomGridCommand : BaseCommand
    {
        public CustomGridCommand() : base("/customgrid", "Sets the grid size for object rotation and translation.")
        {
            AddVariant(args => Toggle());
            AddVariant(args => SetSize((float)args[0]), typeof(float));
        }

        private void Toggle()
        {
            CustomGrid.Enabled = !CustomGrid.Enabled;
            NotificationHelper.NotifySuccess($"Custom grid {(CustomGrid.Enabled ? "enabled" : "disabled")}");
        }

        private void SetSize(float size)
        {
            if (size == 0)
            {
                NotificationHelper.WarnUser("Grid size cannot be 0!");
                return;
            }
            CustomGrid.GridSize = size;
            NotificationHelper.NotifySuccess($"Grid size set to {size}.");
            CustomGrid.Enabled = true;
        }
    }
}
