using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class NoLimit
{
    internal static bool Enabled = false;

    [HarmonyPatch(typeof(ConstraintVisualizer), "Init")]
    [HarmonyPrefix]
    private static bool Init()
    {
        return !Enabled;
    }

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
