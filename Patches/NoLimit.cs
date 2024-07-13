
using HarmonyLib;
using MV.WorldObject;


namespace KogamaTools.patches
{
    internal static class NoLimit
    {
        public static bool Enabled = false;

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
}
