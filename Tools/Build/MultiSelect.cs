using HarmonyLib;
using KogamaTools.Behaviours;
using KogamaTools.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal class MultiSelect : MonoBehaviour
{
    internal static bool ForceSelection = false;
    private static Il2CppSystem.Collections.Generic.HashSet<int> ignoreWOIDs = new();
    private static SelectionController selectionController = null!;

    public MultiSelect(IntPtr handle) : base(handle)
    {
        GameInitChecker.OnGameInitialized += Init;
    }

    private void Init()
    {
        ignoreWOIDs.Add(75579);
        KogamaTools.Instance.AddComponent<MultiSelect>();
        selectionController = MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>().EditModeStateMachine.selectionController;
        KogamaTools.mls.LogInfo(selectionController != null);
    }

    private void Update()
    {
        if (selectionController != null)
        {
            HandleDeselect();
        }
    }
    private static void HandleDeselect()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!MVInputWrapper.GetBooleanControlDown(KogamaControls.PointerSelect)) return;

        bool isShiftHeld = MVInputWrapper.GetBooleanControl(KogamaControls.EditMoveFast);
        VoxelHit vhit = new();

        if (!isShiftHeld)
        {
            if (!ObjectPicker.Pick(ref vhit, ignoreWOIDs))
            {
                DeSelectAll();
            }
            else
            {
                if (!selectionController.selectedIDs.Contains(vhit.woId))
                {
                    DeSelectAllExcept(vhit.woId);
                }
            }
        }
    }

    private static void DeSelectAll()
    {
        if (ForceSelection) return;

        foreach (int num in selectionController.selectedIDs)
        {
            selectionController.WOCM.GetWorldObjectClient(num).DeSelect();
        }
        selectionController.selectedIDs.Clear();
    }

    private static void DeSelectAllExcept(int id)
    {
        if (ForceSelection) return;

        if (!selectionController.selectedIDs.Contains(id))
        {
            DeSelectAll();
            return;
        }
        List<int> deletedIds = new();
        foreach (int num in selectionController.selectedIDs)
        {
            if (num != id)
            {
                selectionController.WOCM.GetWorldObjectClient(num).DeSelect();
                deletedIds.Add(num);
            }
        }

        foreach (int num in deletedIds)
        {
            selectionController.selectedIDs.Remove(num);
        }
    }

    [HarmonyPatch(typeof(SelectionController), "Select", new Type[] { typeof(VoxelHit), typeof(bool), typeof(bool) })]
    [HarmonyPrefix]
    private static void Select(ref bool addToSelection)
    {
        addToSelection = ForceSelection || MVInputWrapper.GetBooleanControl(KogamaControls.EditMoveFast);
    }

    [HarmonyPatch(typeof(ESTranslate), "Exit")]
    [HarmonyPatch(typeof(ESInsert), "Exit")]
    [HarmonyPostfix]
    private static void Exit(EditorStateMachine e, ESTranslate __instance)
    {
        if (selectionController.selectedIDs.Count == 1)
        {
            DeSelectAll();
        }
    }

    [HarmonyPatch(typeof(SelectionController), "DeSelectAll")]
    [HarmonyPatch(typeof(SelectionController), "DeSelectWorldObject")]
    [HarmonyPatch(typeof(SelectionController), "DeSelectAllExcept")]
    [HarmonyPrefix]
    private static bool DeselectPatches()
    {
        return !ForceSelection;
    }
}
