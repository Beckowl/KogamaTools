using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/multiselect")]
[CommandDescription("Allows you to move/rotate multiple objects at once.")]
internal class MultiSelectCommand : BaseCommand
{
    [CommandVariant]
    private void Toggle()
    {
        MultiSelect.ForceSelection = !MultiSelect.ForceSelection;
        NotificationHelper.NotifySuccess($"Multi selection {(MultiSelect.ForceSelection ? "enabled" : "disabled")}.");
    }
}
