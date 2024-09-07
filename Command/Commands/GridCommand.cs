using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/grid")]
[CommandDescription("Sets the grid size for object insertion, rotation and translation.")]
internal class GridCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        CustomGrid.Enabled = !CustomGrid.Enabled;
        NotificationHelper.NotifySuccess($"Custom grid {(CustomGrid.Enabled ? "enabled" : "disabled")}");
    }

    [CommandVariant]
    private void SetSize(float gridSize)
    {
        if (gridSize == 0)
        {
            NotificationHelper.WarnUser("Grid size cannot be 0!");
            return;
        }
        CustomGrid.GridSize = gridSize;
        NotificationHelper.NotifySuccess($"Grid size set to {gridSize}.");
        CustomGrid.Enabled = true;
    }
}
