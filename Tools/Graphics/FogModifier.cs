using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
internal class FogModifier : MonoBehaviour
{
    internal static bool FogEnabled = HasFog();
    internal static float FogDensity = RenderSettings.fogDensity;

    private static bool HasFog()
    {
        ThemeSkybox[] skyboxes = FindObjectsOfType<ThemeSkybox>();
        var skyBox = skyboxes.FirstOrDefault();

        if (skyBox != null)
        {
            return skyBox.FogEnabled;
        }

        return true;
    }

    [HarmonyPatch(typeof(RenderSettings), nameof(RenderSettings.fog), MethodType.Setter)]
    [HarmonyPrefix]
    private static void FogPrefix(ref bool value)
    {
        value = FogEnabled;
    }

    [HarmonyPatch(typeof(RenderSettings), nameof(RenderSettings.fogDensity), MethodType.Setter)]
    [HarmonyPrefix]
    private static void FogDensityPrefix(ref float value)
    {
        value = FogDensity;
    }
}
