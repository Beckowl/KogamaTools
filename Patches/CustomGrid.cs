using HarmonyLib;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(ESTranslate))]
    internal static class CustomGrid
    {
        internal static bool Enabled = false;
        internal static float GridSize = 1f;

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

