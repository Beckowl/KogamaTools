using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Features.Build;

[HarmonyPatch]
internal static class CustomModelScale
{
    internal static float CustomScale = ConfigHelper.GetConfigValue<float>("CustomScale");
    internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("CustomScaleEnabled");

    [HarmonyPatch(typeof(EditorWorldObjectCreation), "OnAddNewPrototype")]
    [HarmonyPrefix]
    private static void OnAddNewPrototype(ref float scale)
    {
        if (Enabled)
        {
            scale = CustomScale;
        }
    }
}