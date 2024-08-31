using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Features.Build;

[HarmonyPatch(typeof(EditorWorldObjectCreation))]
internal static class CustomModelScale
{
    internal static float CustomScale = ConfigHelper.GetConfigValue<float>("CustomScale");
    internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("CustomScaleEnabled");

    [HarmonyPatch("OnAddNewPrototype")]
    [HarmonyPrefix]
    private static void OnAddNewPrototype(ref float scale)
    {
        if (Enabled)
        {
            scale = CustomScale;
        }
    }
}