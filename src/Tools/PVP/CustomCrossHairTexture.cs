using KogamaTools.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace KogamaTools.Tools.PVP;
internal static class CustomCrossHairTexture
{
    internal static string TexturePath = string.Empty;
    internal static void SetTexture()
    {
        Texture2D tex = TextureHelper.LoadPNG(TexturePath);

        if (tex == null)
        {
            NotificationHelper.NotifyError($"Invalid file path {TexturePath}.");
            return;
        }

        CrossHair crosshair = MVGameControllerBase.PlayModeUI.GetCrossHair().Cast<CrossHair>();

        Image image = crosshair.crossHair;
        Sprite sprite = image.sprite;
        Vector2 pivot = sprite.pivot;
        Rect rect = new Rect(0, 0, tex.width, tex.height);

        Sprite newsprite = Sprite.Create(tex, rect, pivot);
        image.sprite = newsprite;
    }
}
