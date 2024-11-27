using KogamaTools.Tools.Build;
using static KogamaTools.Helpers.ModelHelper;

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
        ModelExporter.ExportModel(model);
    }
}
