using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class UnlimitedConfig
{
    internal static bool Enabled = false;
    internal static float MinValue = 0;
    internal static float MaxValue = 1;


    [HarmonyPatch(typeof(SettingsSlider), "Initialize", new Type[] { typeof(string), typeof(float), typeof(float), typeof(float) })]
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

    [HarmonyPatch(typeof(SettingsSlider), "Initialize", new Type[] { typeof(string), typeof(int), typeof(int), typeof(int) })]
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
