using KogamaTools.Helpers;
using KogamaTools.Tools.Build;
using static KogamaTools.Helpers.ModelHelper;
namespace KogamaTools.Command.Commands;

[CommandName("/pastemodel")]
[BuildModeOnly]
internal class PasteModelCommand : BaseCommand
{
    [CommandVariant]
    private void PasteModel()
    {
        MVCubeModelBase targetModel = GetTargetModel();

        if (targetModel == null)
        {
            NotificationHelper.WarnUser("Could not find target cube model.");
            return;
        }

        CopyPasteModel.PasteModel(targetModel);
    }
}
