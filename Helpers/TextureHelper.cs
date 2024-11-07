using UnityEngine;
using static UnityEngine.ImageConversion;

namespace KogamaTools.Helpers;

internal static class TextureHelper
{
    public static Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = null!;

        if (File.Exists(filePath)) // does not work if path has special characters
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(1, 1);
            tex.LoadImage(fileData);
        }
        return tex;
    }

}
