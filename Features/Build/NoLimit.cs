using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Features.Build;

[HarmonyPatch]
internal static class NoLimit
{
    internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("NoLimitEnabled");


    [HarmonyPatch(typeof(ModelingDynamicBoxConstraint), "CanAddCubeAt")]
    [HarmonyPostfix]
    static void CanAddCubeAt(ref bool __result)
    {
        __result |= Enabled;
    }

    [HarmonyPatch(typeof(ModelingBoxCountConstraint), "CanAddCubeAt")]
    [HarmonyPostfix]
    static void CanAddCubeAt2(ref bool __result)
    {
        __result |= Enabled;
    }

    [HarmonyPatch(typeof(ModelingBoxCountConstraint), "CanRemoveCubeAt")]
    [HarmonyPostfix]
    static void CanRemoveCubeAt(ref bool __result)
    {
        __result |= Enabled;
    }
}
