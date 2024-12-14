using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace KogamaTools.Tools.PVP;

[HarmonyPatch]
internal class CustomCrossHairColor
{
    internal static bool Enabled = false;
    internal static Color Color = new(0, 1, 0, 1);

    internal static void SetCrossHairColorFromVec4(System.Numerics.Vector4 color)
    {
        Color.r = color.X;
        Color.g = color.Y;
        Color.b = color.Z;
        Color.a = color.W;
    }

    [HarmonyPatch(typeof(CrossHair), "UpdateCrossHair")]
    [HarmonyPostfix]
    private static void UpdateCrossHair(CrossHair __instance, ref PickupItem pickupItem)
    {
        if (!Enabled)
            return;

        if (__instance.crossHair != null)
        {
            __instance.crossHair.color = Color;
        }
    }
}
