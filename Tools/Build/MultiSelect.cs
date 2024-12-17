using HarmonyLib;
using KogamaTools.Helpers;
using KogamaTools.Tools.Misc;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal class MultiSelect : MonoBehaviour
{
    internal static bool ForceSelection = false;
    private static bool AddToSelection = false;

    private void Awake()
    {
        KeyRemapper.RemapControl<DesktopPlayMode>(KogamaControls.EditMoveDown, KeyCode.C);
    }

    private void Update()
    {
        AddToSelection = MVInputWrapper.DebugGetKey(KeyCode.LeftControl);

        if (RuntimeReferences.EditorStateMachine.selectionController.SelectedWOs.Count <= 1)
        {
            return;
        }

        if (MVInputWrapper.GetBooleanControlDown(KogamaControls.PointerSelect))
        {
            VoxelHit vhit = new();
            if (!ObjectPicker.Pick(ref vhit) || !MVGameControllerBase.WOCM.GetWorldObjectClient(vhit.woId).HasInteractionFlag(InteractionFlags.Selectable))
            {
                DeSelectAll();
            }
        }
    }

    internal static void DeSelectAll()
    {
        var controller = RuntimeReferences.EditorStateMachine.selectionController;
        foreach (int num in controller.selectedIDs)
        {
            controller.WOCM.GetWorldObjectClient(num).DeSelect();
        }
        controller.selectedIDs.Clear();
    }

    private static void DeSelectAllExcept(int id)
    {
        var controller = RuntimeReferences.EditorStateMachine.selectionController;

        if (!controller.selectedIDs.Contains(id))
        {
            DeSelectAll();
        }
        else
        {
            List<int> idsToRemove = new List<int>();

            foreach (int num in controller.selectedIDs)
            {
                if (num != id)
                {
                    controller.WOCM.GetWorldObjectClient(num).DeSelect();
                    idsToRemove.Add(num);
                }
            }

            foreach (int num in idsToRemove)
            {
                controller.selectedIDs.Remove(num);
            }
        }
    }

    [HarmonyPatch(typeof(SelectionController), "Select", new Type[] { typeof(VoxelHit), typeof(bool), typeof(bool) })]
    [HarmonyPrefix]
    private static void Select(SelectionController __instance, ref VoxelHit hit, ref bool addToSelection)
    {
        addToSelection = ForceSelection || AddToSelection;

        if (!addToSelection && !__instance.selectedIDs.Contains(hit.woId) && !ForceSelection)
        {
            DeSelectAllExcept(hit.woId);
        }
    }

    [HarmonyPatch(typeof(SelectionController), "DeSelectAllExcept")]
    [HarmonyPatch(typeof(SelectionController), "DeSelectWorldObject")]
    [HarmonyPatch(typeof(SelectionController), "DeSelectAll")]
    [HarmonyPrefix]
    private static bool CanDeselect(SelectionController __instance)
    {
        if (ForceSelection) { return false; }
        return __instance.SelectedWOs.Count <= 1;
    }
}
