using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal class ObjectGrouper : MonoBehaviour
{
    private static EditorStateMachine editModeStateMachine = RuntimeReferences.EditorStateMachine;
    private static ESWaitForGroup grouper = new();

    internal static void GroupSelectedObjects()
    {
        grouper = new();
        grouper.Enter(editModeStateMachine);
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
            MVWorldObjectClient wo = MVGameControllerBase.WOCM.GetWorldObjectClient(id);

            if (IsObjectProhibited(wo.type))
            {
                NotificationHelper.WarnUser($"Grouping {wo.type.ToString()}s is currently unsupported. World object with ID {id} will not be grouped.");
                __instance.lockList.RemoveAt(i);
                continue;
            }
        }
        __instance.lockCount = __instance.lockList.Count;
        if (__instance.lockCount < 2)
        {
            NotificationHelper.WarnUser("Cannot group less than 2 objects.");
            __instance.abort = true;
            MultiSelect.ForceSelection = false;
            editModeStateMachine.DeSelectAll();
            return;
        }
        NotificationHelper.NotifyUser($"Grouping ({__instance.lockCount}) objects...");
    }

    private static bool IsObjectProhibited(MV.WorldObject.WorldObjectType type)
    {
        return type == MV.WorldObject.WorldObjectType.WorldObjectSpawnerVehicle ||
               type == MV.WorldObject.WorldObjectType.JetPack ||
               type == MV.WorldObject.WorldObjectType.Teleporter ||
               type == MV.WorldObject.WorldObjectType.CollectTheItemCollectable ||
               type == MV.WorldObject.WorldObjectType.CollectTheItemDropOff ||
               type == MV.WorldObject.WorldObjectType.CubeModel;
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