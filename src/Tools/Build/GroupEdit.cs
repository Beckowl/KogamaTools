using HarmonyLib;
using Il2CppInterop.Runtime;
using KogamaTools.Helpers;
using MV.WorldObject;
using UnityEngine;
using UnityEngine.UI;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal class GroupEdit
{

    private static bool isEditingGroup => !RuntimeReferences.EditorStateMachine.ParentGroupIsRoot;
    static GroupEdit()
    {
        CustomContextMenu.AddButton(
            "Edit Group",
            wo => MVGameControllerBase.WOCM.IsType(wo.id, WorldObjectType.Group),
            wo => EnterGroupEdit(wo)
        );
    }

    private static void EnterGroupEdit(MVWorldObjectClient wo)
    {
        NotificationHelper.NotifyUser("You are currently editing a group. Press P to exit group edit mode.");
        MVGroup group = wo.Cast<MVGroup>();
        RuntimeReferences.EditorStateMachine.selectionController.EnterGroup(group);

        MVGameControllerBase.MainCameraManager.BlueModeEnabled = true;
        MVGameControllerBase.MainCameraManager.CurrentCamera.FocusOnObject(wo);

        HighlightObjects(group, true);
    }

    private static void HighlightObjects(MVGroup group, bool highlight)
    {
        foreach (MVWorldObjectClient mvworldObjectClient in group.children.Values)
        {
            foreach (Link link in mvworldObjectClient.OutputLinkRefs)
            {
                LinkObjectScript linkScript = MVGameControllerBase.Game.worldNetwork.links.linkObjects[link.id];

                SharedCubeFunctions.SetLayerRecursively(linkScript.transform, highlight);
            }
            foreach (ObjectLink objectLink in mvworldObjectClient.objectLinkRefs)
            {
                ObjectLinkObjectScript linkScript = MVGameControllerBase.Game.worldNetwork.objectLinks.objectLinkObjects[objectLink.id];

                SharedCubeFunctions.SetLayerRecursively(linkScript.transform, highlight);
            }

            SharedCubeFunctions.SetLayerRecursively(mvworldObjectClient.transform, highlight);
        }
    }

    [HarmonyPatch(typeof(SelectionController), "ExitGroup")]
    [HarmonyPatch(typeof(SelectionController), "ExitGroupToRoot")]
    private static void ExitAllGroups(SelectionController __instance)
    {
        if (!isEditingGroup) return;

        foreach (int groupId in __instance.parentGroups)
        {
            if (groupId != MVGameControllerBase.WOCM.rootGroupId)
            {
                MVGroup group = MVGameControllerBase.WOCM.GetWorldObjectClient<MVGroup>(groupId);
                HighlightObjects(group, false);
            }
        }
    }

    [HarmonyPatch(typeof(DesktopEditModeController), "EnterPlayMode")]
    [HarmonyPrefix]
    private static void EnterPlayMode()
    {
        if (!isEditingGroup) return;

        ExitAllGroups(RuntimeReferences.EditorStateMachine.selectionController);
    }

    [HarmonyPatch(typeof(ContextMenuController), "ShowContextMenu")]
    [HarmonyPrefix]
    private static void ShowContextMenu(ref int woID)
    {
        if (!MVGameControllerBase.WOCM.IsType(woID, WorldObjectType.Group)) return;

        if (MVInputWrapper.DebugGetKey(KeyCode.LeftControl))
        {
            VoxelHit vhit = new();

            if (ObjectPicker.Pick(ref vhit) && vhit.woId != -1)
            {
                MVWorldObjectClient wo = MVGameControllerBase.WOCM.GetWorldObjectClient(vhit.woId);
                if (wo.HasInteractionFlag(InteractionFlags.Selectable))
                {
                    woID = vhit.woId; // show context menu of the object under the mouse cursor instead of the group one
                }
            }
        }
    }

    [HarmonyPatch(typeof(ESTranslate), "Exit")]
    [HarmonyPrefix]
    private static bool Exit(EditorStateMachine e, ESTranslate __instance)
    {
        if (!isEditingGroup) return true;

        MVGameControllerBase.GameEventManager.AvatarCommandsBuildMode.LaserCommands.ChangeState(LaserPointerState.Idle);
        MVGameControllerBase.GameEventManager.AvatarCommandsBuildMode.LaserCommands.SetLaserActiveState(false);
        e.MainCameraManager.IgnoreInputTypes(IgnoreInputTypes.None);
        e.MainCameraManager.TertiaryCameraActive = false;
        e.MainCameraManager.TertiaryCamera.ResetReplacementShader();

        foreach (TranslateData translateData in __instance.translateDatas)
        {
            if (translateData.Wo != null)
            {
                translateData.Wo.SyncPos = translateData.prevGridifiedPosition;
            }
        }

        Cursor.visible = true;
        e.NetworkSelector.RequestReleaseOwnership(e.SelectedIDs);
        e.Data.Add("FromTranslateState", true);

        return false;
    }

    [HarmonyPatch(typeof(ESInsert), "Enter")]
    [HarmonyPrefix]
    private static void Enter(EditorStateMachine e)
    {
        if (!isEditingGroup) return;

        SharedCubeFunctions.SetLayerRecursively(e.SingleSelectedWO.transform, true);
    }

    [HarmonyPatch(typeof(EditorWorldObjectCreation), "OnAddItemFromInventory")]
    [HarmonyPrefix]
    private static unsafe bool OnAddItemFromInventory(InventoryItem item, EditorWorldObjectCreation __instance)
    {
        if (!isEditingGroup) return true;

        KoGaMaPackageClient koGaMaPackageFromItem = EditorWorldObjectCreation.GetKoGaMaPackageFromItem(item);
        if (!__instance.ValidateAddItemFromInventory(koGaMaPackageFromItem))
        {
            koGaMaPackageFromItem.Destroy();
            return false;
        }

        int num = -1;
        if (MVGameControllerBase.WOCM.GetUnmodifiedWorldObject(koGaMaPackageFromItem, ref num))
        {
            Debug.Log("Found wo for cloning");
            MVWorldObjectClient worldObjectClient = MVGameControllerBase.WOCM.GetWorldObjectClient(num);
            __instance.Clone(worldObjectClient, false, false, true);
        }
        else
        {
            Debug.Log("Creating new wo");

            __instance.esm.Event = EditorEvent.ESWaitForSelect.ToIl2Cpp();

            Quaternion identity = Quaternion.identity;
            MVGameControllerBase.OperationRequests.AddItemToWorld(item.itemID, __instance.esm.ParentGroupID, Vector3.up * 10f, identity, false, true, false);
        }
        koGaMaPackageFromItem.Destroy();

        return false;
    }

    [HarmonyPatch(typeof(Links), "AddLink")]
    [HarmonyPostfix]
    private static void AddLink(ref Link link)
    {
        if (!MVGameControllerBase.IsInitialized || !isEditingGroup) return;

        LinkObjectScript linkScript = MVGameControllerBase.Game.worldNetwork.links.linkObjects[link.id];
        SharedCubeFunctions.SetLayerRecursively(linkScript.transform, true);
    }
}
