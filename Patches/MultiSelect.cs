using HarmonyLib;
namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(SelectionController))]
    internal static class MultiSelect
    {
        internal static bool ForceSelection = false;

        [HarmonyPatch("Select", [typeof(VoxelHit), typeof(bool), typeof(bool)])]
        [HarmonyPrefix]
        private static void Select(ref bool addToSelection)
        {
            addToSelection = MVInputWrapper.DebugGetKey(UnityEngine.KeyCode.LeftShift) || ForceSelection;
        }

        [HarmonyPatch("DeSelectAll")]
        [HarmonyPrefix]
        private static bool DeSelectAll()
        {
            return CanDeselect();
        }

        [HarmonyPatch("DeSelectWorldObject")]
        [HarmonyPrefix]
        private static bool DeSelectWorldObject()
        {
            return CanDeselect();
        }

        [HarmonyPatch("DeSelectAllExcept")]
        [HarmonyPrefix]
        private static bool DeSelectAllExcept()
        {
            return CanDeselect();
        }

        private static bool CanDeselect()
        {
            return !ForceSelection && !MVInputWrapper.DebugGetKey(UnityEngine.KeyCode.LeftShift);
        }

    }
}
