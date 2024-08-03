using System;
using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(CrossHair))]
    internal static class CustomCrossHairColor
    {
        internal static bool CustomColorEnabled = false;
        internal static Color Color = Color.green;

        [HarmonyPatch("UpdateCrossHair")]
        [HarmonyPostfix]
        private static void UpdateCrossHair(CrossHair __instance)
        {
            if (CustomColorEnabled)
            {
                if (__instance.crossHair != null)
                {
                    __instance.crossHair.color = Color;
                }
            }     
        }
    }
}
