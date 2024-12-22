using HarmonyLib;
using KogamaTools.Helpers;
using KogamaTools.Tools.Misc;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class CustomWOScale
{
    internal static bool Enabled = false;
    internal static Vector3 Scale = Vector3.one;

    private static MVGroup scaledGroup = null!;
    private static bool waitingForGroup = false;

    static CustomWOScale()
    {
        WOReciever.OnWORecieved += OnWORecieved;
    }

    internal static void RequestNewGroupIfNecessary()
    {
        if (scaledGroup == null)
        {
            RequestNewGroup();
            return;
        }

        if (scaledGroup.scale != Scale)
        {
            DeleteScaledGroup();
            RequestNewGroup();
        }
    }

    private static void RequestNewGroup()
    {
        MVGameControllerBase.OperationRequests.RequestBuiltInItem(
            MV.Common.BuiltInItem.Group,
            RuntimeReferences.EditorStateMachine.ParentGroupID,
            new Il2CppSystem.Collections.Generic.Dictionary<Il2CppSystem.Object, Il2CppSystem.Object>(),
            Vector3.zero,
            Quaternion.identity,
            Scale,
            true,
            false
            );

        waitingForGroup = true;
    }

    private static void DeleteScaledGroup()
    {
        MVGameControllerBase.OperationRequests.UnregisterWorldObject(scaledGroup.id);
    }

    private static void OnWORecieved(MVWorldObjectClient root, int instigatorActorNumber)
    {
        if (instigatorActorNumber == MVGameControllerBase.LocalPlayer.ActorNr)
        {
            if (Enabled && MVGameControllerBase.WOCM.IsType(root.id, MV.WorldObject.WorldObjectType.Group) && waitingForGroup)
            {
                scaledGroup = root.Cast<MVGroup>();
                waitingForGroup = false;
            }
        }
    }

    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "AddItemToWorld")]
    [HarmonyPrefix]
    private static void AddItemToWorld(ref int groupId)
    {
        if (Enabled && scaledGroup != null)
        {
            groupId = scaledGroup.id;
        }
    }

    [HarmonyPatch(typeof(ESInsert), "Exit")]
    [HarmonyPostfix]
    private static void Exit(ref ESInsert __instance, ref EditorStateMachine e)
    {
        if (Enabled)
        {
            if (e.SingleSelectedWO.groupId == scaledGroup.id)
            {
                e.selectionController.ExitGroup();
                RequestNewGroup();
            }
        }
    }

    [HarmonyPatch(typeof(ContextMenuController), "ShowContextMenu")]
    [HarmonyPrefix]
    private static void ShowContextMenu(ref int woID)
    {
        if (MVGameControllerBase.WOCM.IsType(woID, MV.WorldObject.WorldObjectType.Group))
        {
            MVGroup group = MVGameControllerBase.WOCM.GetWorldObjectClient(woID).Cast<MVGroup>();

            if (group.children.Count == 1)
            {
                // can't use linq
                var enumerator = group.children.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    woID = enumerator.Current.value.id;
                }
            }
        }
    }
}
