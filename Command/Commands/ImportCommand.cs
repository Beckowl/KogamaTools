using KogamaTools.Tools.Build;

namespace KogamaTools.Command.Commands;
[CommandName("/import")]
[CommandDescription("Imports an external cube model into the game.")]
internal class ImportCommand : BaseCommand
{
    [CommandVariant]
    private void ImportModel(string path)
    {
        ModelImporter.LoadModelData(path);
    }
}
