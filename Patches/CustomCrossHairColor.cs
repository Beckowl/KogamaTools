using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Patches
{
    internal static class CustomCrossHairColor
    {
        internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("CustomCrossHairColorEnabled");
        internal static System.Numerics.Vector3 CrossHairColor = new System.Numerics.Vector3(0, 1, 0);

        static CustomCrossHairColor()
        {
            SetColorFromHTMLString(ConfigHelper.GetConfigValue<string>("CrosshairColor"));
        }

        internal static bool SetColorFromHTMLString(string htmlString)
        {
            bool success = ColorUtility.TryParseHtmlString(htmlString, out Color color);
            if (success)
            {
                CrossHairColor.X = color.r; CrossHairColor.Y = color.g; CrossHairColor.Z = color.b;
            }
            return success;
        }

        [HarmonyPatch(typeof(CrossHair), "UpdateCrossHair")]
        [HarmonyPostfix]
        private static void UpdateCrossHair(CrossHair __instance, ref PickupItem pickupItem)
        {
            if (Enabled)
            {
                if (__instance.crossHair != null)
                {
                    Color customcolor = new Color(CrossHairColor.X, CrossHairColor.Y, CrossHairColor.Z);
                    __instance.crossHair.color = customcolor;
                }
            }
        }
    }
}
