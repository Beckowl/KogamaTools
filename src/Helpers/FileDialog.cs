using Il2CppInterop.Runtime;
using NativeFileDialogSharp;

namespace KogamaTools.Helpers;
internal static class FileDialog
{
    internal static void OpenFile(string? filterList, string? defaultPath, Action<DialogResult> callback)
    {
        Task.Run(() =>
        {
            var thread = IL2CPP.il2cpp_thread_attach(IL2CPP.il2cpp_domain_get());
            DialogResult result = Dialog.FileOpen(filterList, defaultPath);

            callback?.Invoke(result);
            IL2CPP.il2cpp_thread_detach(thread);
        });
    }
}
