using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(ESTranslate))]
    internal static class CustomGrid
    {
        internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("CustomGridEnabled");
        internal static float GridSize = ConfigHelper.GetConfigValue<float>("GridSize");

        [HarmonyPatch("Execute")]
        [HarmonyPrefix]
        private static void Execute(ESTranslate __instance)
        {
            if (Enabled)
            {
                __instance.gridSize = GridSize;
            }
        }
    }
}

