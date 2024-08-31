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

    [HarmonyPatch(typeof(SelectionController), "DeSelectAll")]
    [HarmonyPrefix]
    private static bool DeSelectAll()
    {
        return CanDeselect();
    }

    [HarmonyPatch(typeof(SelectionController), "DeSelectWorldObject")]
    [HarmonyPrefix]
    private static bool DeSelectWorldObject()
    {
        return CanDeselect();
    }

    [HarmonyPatch(typeof(SelectionController), "DeSelectAllExcept")]
    [HarmonyPrefix]
    private static bool DeSelectAllExcept()
    {
        return CanDeselect();
    }

    private static bool CanDeselect()
    {
        return !ForceSelection;
    }
}
