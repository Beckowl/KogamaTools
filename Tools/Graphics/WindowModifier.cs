using UnityEngine;

namespace KogamaTools.Tools.Graphics;
internal static class WindowModifier
{
    internal static int Width = UnityEngine.Screen.width;
    internal static int Height = UnityEngine.Screen.height;
    internal static FullScreenMode MODE;
    internal static string[] FullScreenModes = Enum.GetNames(typeof(FullScreenMode));
    internal static int SelectedFullScreenMode = (int)FullScreenMode.Windowed;

    internal static void ApplyResolution()
    {
        UnityEngine.Screen.SetResolution(Width, Height, (FullScreenMode)SelectedFullScreenMode);
    }
}
