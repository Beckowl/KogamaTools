using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
internal static class FogModifier
{
    internal static bool FogEnabled = HasFog();
    internal static float FogDensity = MVGameControllerBase.SkyboxManager.currentFogDensity;

    internal static void ApplyChanges()
    {
        RenderSettings.fog = FogEnabled;
        RenderSettings.fogDensity = FogDensity;
    }

    private static bool HasFog()
    {
        ThemeSkybox[] skyboxes = UnityEngine.Object.FindObjectsOfType<ThemeSkybox>();
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
