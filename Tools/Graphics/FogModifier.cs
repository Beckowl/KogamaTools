using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
internal static class FogModifier
{
    internal static bool FogEnabled = false;
    [HarmonyPatch(typeof(RenderSettings), nameof(RenderSettings.fog), MethodType.Setter)]
    [HarmonyPrefix]
    private static void FogPrefix(ref bool value)
    {
        value = FogEnabled;
    }
}
