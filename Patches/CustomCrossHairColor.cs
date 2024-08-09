using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(CrossHair))]
    internal static class CustomCrossHairColor
    {
        internal static bool CustomColorEnabled = ConfigHelper.GetConfigValue<bool>("CustomCrossHairColorEnabled");
        internal static Color Color = Color.green;

        static CustomCrossHairColor()
        {
            SetColorFromHTMLString(ConfigHelper.GetConfigValue<string>("CrosshairColor"));
        }

        internal static bool SetColorFromHTMLString(string htmlString)
        {
            bool success = ColorUtility.TryParseHtmlString(htmlString, out Color color);
            if (success)
            {
                Color = color;
            }
            return success;
        }

        [HarmonyPatch("UpdateCrossHair")]
        [HarmonyPostfix]
        private static void UpdateCrossHair(CrossHair __instance, ref PickupItem pickupItem)
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
