using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal class ObjectGrouper : MonoBehaviour
{
    internal static bool CanGroup = false;
    private static FSMEntity editModeStateMachine = null!;
    private static ESWaitForGroup grouper = new();

    internal static void OnGameInitialized()
    {
        editModeStateMachine = MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>().EditModeStateMachine;
    }

    internal static void GroupSelectedObjects()
    {
        if (editModeStateMachine != null)
        {
            grouper.Enter(editModeStateMachine);
        }
    }

    private void Update()
    {
        if (editModeStateMachine != null)
        {
            if (editModeStateMachine.lockState)
            {
                grouper.Execute(editModeStateMachine);
            }
        }
    }

    [HarmonyPatch(typeof(ESWaitForGroup), "WOCM_OnTransferWosResponse")]
    [HarmonyPrefix]
    private static void WOCM_OnTransferWosResponse(OnTransferWosResponseEventArgs e)
    {
        if (e.success)
        {
            NotificationHelper.NotifySuccess("Objects grouped successfully.");
        }
        else
        {
            NotificationHelper.NotifyError("Failed to group objects.");
        }
    }
}