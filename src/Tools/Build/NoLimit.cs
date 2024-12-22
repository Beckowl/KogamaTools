using HarmonyLib;

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
    [HarmonyPatch(typeof(ModelingBoxCountConstraint), "CanAddCubeAt")]
    [HarmonyPatch(typeof(ModelingBoxCountConstraint), "CanRemoveCubeAt")]
    [HarmonyPostfix]
    private static void NoLimitPatches(ref bool __result)
    {
        __result |= Enabled;
    }
}
