using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Patches;

[HarmonyPatch(typeof(CrossHair))]
internal static class CustomCrossHairColor
{
    internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("CustomCrossHairColorEnabled");
    internal static Color CrossHairColor = new Color(0, 1, 0, 1);

    static CustomCrossHairColor()
    {
        if (ColorHelper.TryParseColorString(ConfigHelper.GetConfigValue<string>("CrosshairColor"), out CrossHairColor))
        {
        }
    }

    internal static void SetColorFromVector4(System.Numerics.Vector4 color)
    {
        CrossHairColor.r = color.X;
        CrossHairColor.g = color.Y;
        CrossHairColor.b = color.Z;
        CrossHairColor.a = color.W;
    }

    [HarmonyPatch("UpdateCrossHair")]
    [HarmonyPostfix]
    private static void UpdateCrossHair(CrossHair __instance, ref PickupItem pickupItem)
    {
        if (Enabled)
        {
            if (__instance.crossHair != null)
            {
                __instance.crossHair.color = CrossHairColor;
            }
        }
    }
}
