using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/group")]
[CommandDescription("Groups all selected objects.")] // no shit sherlock
internal class GroupCommand : BaseCommand
{
    [CommandVariant]
    private void GroupSelected()
    {
        ObjectGrouper.GroupSelectedObjects();
        NotificationHelper.NotifySuccess("Objects grouped successfuly.");
    }
}
