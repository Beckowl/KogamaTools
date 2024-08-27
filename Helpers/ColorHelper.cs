using UnityEngine;

namespace KogamaTools.Helpers;

internal static class ColorHelper
{
    internal static System.Numerics.Vector4 ToVector4(Color color)
    {
        return new System.Numerics.Vector4(color.r, color.g, color.b, color.a);
    }

    internal static Color ToUnityColor(System.Numerics.Vector4 color)
    {
        return new Color(color.X, color.Y, color.Z, color.W);
    }

    // TODO: Better parsing
    internal static bool TryParseColorString(string colorString, out Color result)
    {
        return ColorUtility.TryParseHtmlString(colorString, out result);
    }
}
