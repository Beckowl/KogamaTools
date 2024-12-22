using HarmonyLib;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class CustomGrid
{
    internal static bool Enabled = false;
    internal static float GridSize = 1;

    [HarmonyPatch(typeof(ESTranslate), "Execute")]
    [HarmonyPrefix]
    private static void Execute(ESTranslate __instance)
    {
        if (Enabled)
        {
            __instance.gridSize = GridSize;
        }
    }

    [HarmonyPatch(typeof(MVWorldObjectClient), "GetClosestGridPoint")]
    [HarmonyPrefix]
    private static void GetClosestGridPoint(ref float gridSize)
    {
        if (Enabled)
        {
            gridSize = GridSize;
        }
    }

}

