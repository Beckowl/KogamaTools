using System;

using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace KogamaTools.Patches
{
    internal class UnlimitedConfig
    {
        public static bool Enabled = ConfigHelper.GetConfigValue<bool>("UnlimitedConfigEnabled");
        public static float MinValue = ConfigHelper.GetConfigValue<float>("MinValue");
        public static float MaxValue = ConfigHelper.GetConfigValue<float>("MaxValue");

        [HarmonyPatch(typeof(SettingsSlider))]
        private static class SettingsSliderPatch
        {
            [HarmonyPatch("Initialize", new Type[] { typeof(string), typeof(float), typeof(float), typeof(float) })]
            [HarmonyPrefix]
            static void Initialize(ref float value, ref float minValue, ref float maxValue)
            {
                if (Enabled)
                {
                    minValue = MinValue;
                    maxValue = MaxValue;
                    value = Mathf.Clamp(value, MinValue, MaxValue);
                }
            }

            [HarmonyPatch("Initialize", new Type[] { typeof(string), typeof(int), typeof(int), typeof(int) })]
            [HarmonyPrefix]
            static void Initialize(ref int value, ref int minValue, ref int maxValue)
            {
                if (Enabled)
                {
                    minValue = (int)MinValue;
                    maxValue = (int)MaxValue;
                    value = (int)Mathf.Clamp(value, MinValue, MaxValue);
                }
            }
        }

        // code below is commented out because it causes a crash
        // I will try to find a better solution later
        /*
        [HarmonyPatch(typeof(InputField))]
        private static class InputFieldPatch
        {
            [HarmonyPatch("characterLimit", MethodType.Getter)]
            [HarmonyPostfix]
            static void CharacterLimit(ref int __result)
            {
                if (Enabled)
                {
                    __result = int.MaxValue;
                }
                KogamaTools.mls.LogInfo(__result);
            }
        }
        */
    }
}
