using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
internal class FogModifier : MonoBehaviour
{
    internal static bool FogEnabled = HasFog();

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
}
