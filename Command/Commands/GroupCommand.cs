using KogamaTools.Helpers;
using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;

[CommandName("/group")]
[CommandDescription("Groups all selected objects.")]
[BuildModeOnly]
internal class GroupCommand : BaseCommand
{
    [CommandVariant]
    private void GroupSelected()
    {
        ObjectGrouper.GroupSelectedObjects();
    }
}
