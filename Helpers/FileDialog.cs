using NativeFileDialogSharp;

namespace KogamaTools.Helpers;
internal static class FileDialog
{
    internal static void OpenFile(string? filterList, string? defaultPath, Action<DialogResult> callback)
    {
        Task.Run(() =>
        {
            DialogResult result = Dialog.FileOpen(filterList, defaultPath);
            callback?.Invoke(result);
        });
    }
}
