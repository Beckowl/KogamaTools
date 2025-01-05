using HarmonyLib;
using KogamaTools.Config;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
[Section("Build")]
internal static class CustomGrid
{
    [Bind] internal static bool Enabled = false;
    [Bind] internal static float GridSize = 1;

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

