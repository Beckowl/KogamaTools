using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KogamaTools.Tools.Graphics;
internal static class ResolutionModifier
{
    internal static int[] resolution = new int[2] { UnityEngine.Screen.width, UnityEngine.Screen.height };
    internal static bool fullscreen = FullScreenController.fullScreen;

    internal static void ApplyResolution()
    {
        UnityEngine.Screen.SetResolution(resolution[0], resolution[1], fullscreen);
    }
}
