using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class CustomGrid
{
    internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("CustomGridEnabled");
    internal static float GridSize = ConfigHelper.GetConfigValue<float>("GridSize");


    [HarmonyPatch(typeof(ESTranslate), "Execute")]
    [HarmonyPrefix]
    static void Execute(ESTranslate __instance)
    {
        if (Enabled)
        {
            __instance.gridSize = GridSize;
        }
    }



    [HarmonyPatch(typeof(MVWorldObjectClient), "GetClosestGridPoint")]
    [HarmonyPrefix]
    static void GetClosestGridPoint(ref float gridSize)
    {
        if (Enabled)
        {
            gridSize = GridSize;
        }
    }

}

