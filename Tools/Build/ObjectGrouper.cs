using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal class ObjectGrouper : MonoBehaviour
{
    internal static bool CanGroup = false;
    private static EditorStateMachine editModeStateMachine = null!;
    private static ESWaitForGroup grouper = new();

    internal static void OnGameInitialized()
    {
        if (MVGameControllerBase.GameMode != MV.Common.MVGameMode.Play)
        {
            editModeStateMachine = MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>().EditModeStateMachine;
        }
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

    [HarmonyPatch(typeof(MVWorldObjectClientManagerNetwork), "SetOwnerInHierarchy")]
    [HarmonyPrefix]
    private static bool SetOwnerInHierarchy(MVWorldObjectClientManagerNetwork __instance, int id, int actorNr)
    {
        __instance.worldObjects[id].OwnerActorNr = actorNr;
        if (__instance.worldObjects[id].GetType() == typeof(MVGroup))
        {
            foreach (MVWorldObjectClient mvworldObjectClient in ((MVGroup)__instance.worldObjects[id]).Children)
            {
                mvworldObjectClient.OwnerActorNr = actorNr;
            }
        }
        return false;
    }

    [HarmonyPatch(typeof(ESWaitForGroup), "Enter")]
    [HarmonyPostfix]
    private static void Enter(ESWaitForGroup __instance)
    {
        for (int i = __instance.lockList.Count - 1; i >= 0; i--)
        {
            int id = __instance.lockList[i];
            if (MVGameControllerBase.WOCM.IsType(id, MV.WorldObject.WorldObjectType.CubeModel))
            {
                __instance.lockList.RemoveAt(i);
                NotificationHelper.WarnUser($"Grouping models is currently not supported. World object {id} will not be grouped.");
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
            MultiSelect.ForceSelection = false;
            editModeStateMachine.DeSelectAll();
        }
        else
        {
            NotificationHelper.NotifyError("Failed to group objects.");
        }
    }
}