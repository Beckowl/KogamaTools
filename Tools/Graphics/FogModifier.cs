using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
internal static class FogModifier
{
    internal static bool FogEnabled = HasFog();
    internal static bool UseCustomFogDensity = false;
    internal static float FogDensity = MVGameControllerBase.SkyboxManager.currentFogDensity;

    internal static void ApplyChanges()
    {
        RenderSettings.fog = FogEnabled;

        if (UseCustomFogDensity)
        {
            RenderSettings.fogDensity = FogDensity;
        }
        else
        {
            ResetFog();
        }
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

    private static void ResetFog()
    {
        if (MVGameControllerBase.SkyboxManager.enabled)
        {
            SkyboxManager skyboxManager = MVGameControllerBase.SkyboxManager;
            skyboxManager.SetColor(skyboxManager.currentColor, skyboxManager.currentSunAngle, skyboxManager.currentFogDensity);
        }
        else
        {
            ThemeModifier.GetCurrentTheme().Cast<CloudyThemeBase>().skybox.ApplyRenderSettings();
        }
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
        if (UseCustomFogDensity)
            value = FogDensity;
    }
}
