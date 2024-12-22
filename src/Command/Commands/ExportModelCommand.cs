using KogamaTools.Tools.Build;
using static KogamaTools.Helpers.ModelHelper;
using static KogamaTools.Helpers.NotificationHelper;

namespace KogamaTools.Command.Commands;
[CommandName("/exportmodel")]
[BuildModeOnly]
[CommandDescription("Exports a model to a file on your PC.")]
internal class ExportModelCommand : BaseCommand
{
    [CommandVariant]
    private void ExportModel()
    {
        MVCubeModelBase model = GetTargetModel();

        if (model == null)
        {
            WarnUser("Could not get target model.");
            return;
        }

        ModelExporter.ExportModel(model);
    }
}
