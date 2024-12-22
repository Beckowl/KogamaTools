using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/multiselect")]
[CommandName("/msel")]
[CommandDescription("Allows you to move/rotate multiple objects at once.")]
[BuildModeOnly]
internal class MultiSelectCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        MultiSelect.ForceSelection = !MultiSelect.ForceSelection;
        NotificationHelper.NotifySuccess($"Multi selection {(MultiSelect.ForceSelection ? "enabled" : "disabled")}.");
    }
}
