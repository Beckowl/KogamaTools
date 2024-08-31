using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Features.Build;

internal static class CustomGrid
{
    internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("CustomGridEnabled");
    internal static float GridSize = ConfigHelper.GetConfigValue<float>("GridSize");

    [HarmonyPatch(typeof(ESTranslate))]
    private static class ESTranslatePatch
    {
        [HarmonyPatch("Execute")]
        [HarmonyPrefix]
        static void Execute(ESTranslate __instance)
        {
            if (Enabled)
            {
                __instance.gridSize = GridSize;
            }
        }
    }

    [HarmonyPatch(typeof(MVWorldObjectClient))]
    private static class MVWorldObjectClientPatch
    {
        [HarmonyPatch("GetClosestGridPoint")]
        [HarmonyPrefix]
        static void GetClosestGridPoint(ref float gridSize)
        {
            if (Enabled)
            {
                gridSize = GridSize;
            }
        }
    }
}

