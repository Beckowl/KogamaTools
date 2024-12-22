using KogamaTools.Behaviours;
using KogamaTools.Helpers;
using KogamaTools.Tools.Build;
using NativeFileDialogSharp;

namespace KogamaTools.Command.Commands;
[CommandName("/importmodel")]
[BuildModeOnly]
[CommandDescription("Imports a cube model from a file into the game.")]
internal class ImportModelCommand : BaseCommand
{

    [CommandVariant]
    private void ImportModel()
    {
        FileDialog.OpenFile("ktm", ModelHelper.ModelsPath, OpenFileCallback);
    }

    [CommandVariant]
    private void ImportModel(string filePath)
    {
        ModelImporter.ImportModel(filePath);
    }

    private void OpenFileCallback(DialogResult result)
    {
        if (result.IsOk)
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => ModelImporter.ImportModel(result.Path));
        }
    }
}
