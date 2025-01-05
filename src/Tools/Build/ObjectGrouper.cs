using HarmonyLib;
using KogamaTools.Behaviours;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal class ObjectGrouper : MonoBehaviour
{
    private static ESWaitForGroup grouper = new();

    private void Awake()
    {
        if (MVGameControllerBase.GameMode != MV.Common.MVGameMode.Edit)
        {
            Destroy(this);
            return;
        }

        HotkeySubscriber.Subscribe(KeyCode.G, OnGroupKeyPressed);
    }

    internal static void GroupSelectedObjects()
    {
        if (MVGameControllerBase.GameMode != MV.Common.MVGameMode.Edit) return;

        grouper = new();
        grouper.Enter(RuntimeReferences.EditorStateMachine);
    }

    private void Update()
    {
        if (RuntimeReferences.EditorStateMachine.lockState)
        {
            grouper.Execute(RuntimeReferences.EditorStateMachine);
        }

    }

    private void OnGroupKeyPressed()
    {
        if (MVInputWrapper.DebugGetKey(KeyCode.LeftControl)) // cursed way to do shortcuts, too lazy to add shortcut functionality rn
        {
            GroupSelectedObjects();
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
            MultiSelect.DeSelectAll();

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
            MultiSelect.DeSelectAll();
        }
        else
        {
            NotificationHelper.NotifyError("Failed to group objects.");
        }
    }
}