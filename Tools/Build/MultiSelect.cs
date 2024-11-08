using HarmonyLib;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class MultiSelect
{
    internal static bool ForceSelection = false;

    [HarmonyPatch(typeof(SelectionController), "Select", new Type[] { typeof(VoxelHit), typeof(bool), typeof(bool) })]
    [HarmonyPrefix]
    private static void Select(ref bool addToSelection)
    {
        addToSelection = ForceSelection;
    }

    [HarmonyPatch(typeof(SelectionController), "DeSelectAllExcept")]
    [HarmonyPatch(typeof(SelectionController), "DeSelectWorldObject")]
    [HarmonyPatch(typeof(SelectionController), "DeSelectAll")]
    [HarmonyPrefix]
    private static bool CanDeselect()
    {
        return !ForceSelection;
    }
}
