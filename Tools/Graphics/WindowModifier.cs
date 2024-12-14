using UnityEngine;

namespace KogamaTools.Tools.Graphics;
internal static class WindowModifier
{
    internal static int Width = Screen.width;
    internal static int Height = Screen.height;
    internal static FullScreenMode screenMode = FullScreenMode.Windowed;

    internal static void ApplyResolution()
    {
        Screen.SetResolution(Width, Height, screenMode);
    }
}
