using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Features.Build;

internal static class NoLimit
{
    internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("NoLimitEnabled");

    [HarmonyPatch(typeof(ModelingDynamicBoxConstraint))]
    private static class ModelingDynamicBoxConstraintPatch
    {
        [HarmonyPatch("CanAddCubeAt")]
        [HarmonyPostfix]
        static void CanAddCubeAt(ref bool __result)
        {
            __result |= Enabled;
        }
    }

    [HarmonyPatch(typeof(ModelingBoxCountConstraint))]
    private static class ModelingBoxCountPatch
    {
        [HarmonyPatch("CanAddCubeAt")]
        [HarmonyPostfix]
        static void CanAddCubeAt(ref bool __result)
        {
            __result |= Enabled;
        }

        [HarmonyPatch("CanRemoveCubeAt")]
        [HarmonyPostfix]
        static void CanRemoveCubeAt(ref bool __result)
        {
            __result |= Enabled;
        }
    }
}
