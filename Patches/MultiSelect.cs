using HarmonyLib;
namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(SelectionController))]
    internal class MultiSelect
    {
        internal static bool ForceSelection = false;

        [HarmonyPatch("Select", [typeof(VoxelHit), typeof(bool), typeof(bool)])]
        [HarmonyPrefix]
        static void Select(ref bool addToSelection)
        {
            addToSelection = MVInputWrapper.DebugGetKey(UnityEngine.KeyCode.LeftShift) || ForceSelection;
        }

        [HarmonyPatch("DeSelectAll")]
        [HarmonyPrefix]
        static bool DeSelectAll()
        {
            KogamaTools.mls.LogInfo("DeSelectAll");
            return CanDeselect();
        }

        [HarmonyPatch("DeSelectWorldObject")]
        [HarmonyPrefix]
        static bool DeSelectWorldObject()
        {
            KogamaTools.mls.LogInfo("DeSelectWO");
            return CanDeselect();
        }

        [HarmonyPatch("DeSelectAllExcept")]
        [HarmonyPrefix]
        static bool DeSelectAllExcept()
        {
            KogamaTools.mls.LogInfo("DeSelectAllExcept");
            return CanDeselect();
        }

        private static bool CanDeselect()
        {
            return !ForceSelection && !MVInputWrapper.DebugGetKey(UnityEngine.KeyCode.LeftShift);
        }

    }
}
