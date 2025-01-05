using BepInEx;
using KogamaTools.Behaviours;
using KogamaTools.Config;
using KogamaTools.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace KogamaTools.Tools.PVP;
[Section("PVP")]
internal static class CustomCrossHairTexture
{
    [Bind] internal static string TexturePath = string.Empty;

    [InvokeOnInit]
    internal static void SetTexture()
    {
        if (TexturePath.IsNullOrWhiteSpace()) return;

        Texture2D tex = TextureHelper.LoadPNG(TexturePath);

        if (tex == null)
        {
            NotificationHelper.NotifyError($"Could not load custom crosshair: Invalid file path {TexturePath}.");
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
