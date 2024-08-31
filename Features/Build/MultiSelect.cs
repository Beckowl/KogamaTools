using HarmonyLib;

namespace KogamaTools.Features.Build;

[HarmonyPatch(typeof(SelectionController))]
internal static class MultiSelect
{
    internal static bool ForceSelection = false;


    [HarmonyPatch("Select", new Type[] { typeof(VoxelHit), typeof(bool), typeof(bool) })]
    [HarmonyPrefix]
    private static void Select(ref bool addToSelection)
    {
        addToSelection = ForceSelection;
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
        return !ForceSelection;
    }
}
