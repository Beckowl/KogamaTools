﻿using HarmonyLib;
using KogamaTools.Config;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
[Section("Build")]
internal static class UnlimitedConfig
{
    [Bind] internal static bool Enabled = false;
    [Bind] internal static bool ClampValues = true;
    [Bind] internal static float MinValue = 0;
    [Bind] internal static float MaxValue = 1;

    [HarmonyPatch(typeof(SettingsSlider), "Initialize", new Type[] { typeof(string), typeof(float), typeof(float), typeof(float) })]
    [HarmonyPrefix]
    private static void Initialize(ref float value, ref float minValue, ref float maxValue)
    {
        if (Enabled)
        {
            minValue = MinValue;
            maxValue = MaxValue;

            if (ClampValues)
                value = Mathf.Clamp(value, MinValue, MaxValue);
        }
    }

    [HarmonyPatch(typeof(SettingsSlider), "Initialize", new Type[] { typeof(string), typeof(int), typeof(int), typeof(int) })]
    [HarmonyPrefix]
    private static void Initialize(ref int value, ref int minValue, ref int maxValue)
    {
        if (Enabled)
        {
            minValue = (int)MinValue;
            maxValue = (int)MaxValue;

            if (ClampValues)
                value = (int)Mathf.Clamp(value, MinValue, MaxValue);
        }
    }
}
