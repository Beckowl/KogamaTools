using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;
[CommandName("/importmodel")]
[BuildModeOnly]
[CommandDescription("Imports a cube model from a file into the game.")]
internal class ImportModelCommand : BaseCommand
{
    [CommandVariant]
    private void ImportModel(string filePath)
    {
        ModelImporter.ImportModel(filePath);
    }
}
