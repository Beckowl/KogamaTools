using HarmonyLib;
using KogamaTools.Helpers;
using MV.WorldObject;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal class GroupEdit
{
    internal static Stack<MVGroup> GroupStack = new();
    private static bool IsEditingGroup => GroupStack.Count > 0;

    static GroupEdit()
    {
        CustomContextMenu.AddButton(
            "Edit Group",
            wo => MVGameControllerBase.WOCM.IsType(wo.id, WorldObjectType.Group) && !GroupStack.Contains(wo),
            wo => EnterGroupEdit(wo)
        );
    }

    private static void EnterGroupEdit(MVWorldObjectClient wo)
    {
        if (wo.id == MVGameControllerBase.WOCM.RootGroup.Id) return;

        NotificationHelper.NotifyUser("You are currently editing a group. Press P to exit group edit mode.");

        EditorStateMachine esm = RuntimeReferences.EditorStateMachine;

        wo.OnEnterObject(esm);
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
            mvworldObjectClient.gameObject.layer = 14;
            SharedCubeFunctions.SetLayerRecursively(mvworldObjectClient.transform, highlight);
        }
    }

    private static void ExitAllGroups()
    {
        EditorStateMachine esm = RuntimeReferences.EditorStateMachine;

        while (GroupStack.TryPop(out MVGroup? group))
        {
            HighlightObjects(group, false);
            group.OnExitObject(esm);
        }
    }

    [HarmonyPatch(typeof(MVGroup), "OnEnterObject")]
    [HarmonyPostfix]
    private static void OnEnterObject(MVGroup __instance)
    {
        HighlightObjects(__instance, true);
        GroupStack.Push(__instance);
    }

    [HarmonyPatch(typeof(DesktopEditModeController), "EnterPlayMode")]
    [HarmonyPrefix]
    private static void EnterPlayMode()
    {
        ExitAllGroups();
    }

    [HarmonyPatch(typeof(Links), "AddLink")]
    [HarmonyPostfix]
    private static void AddLink(ref Link link)
    {
        if (!IsEditingGroup) return;

        LinkObjectScript linkScript = MVGameControllerBase.Game.worldNetwork.links.linkObjects[link.id];
        SharedCubeFunctions.SetLayerRecursively(linkScript.transform, true);
    }

    [HarmonyPatch(typeof(ESTranslate), "Exit")]
    [HarmonyPrefix]
    private static bool Exit(EditorStateMachine e, ESTranslate __instance)
    {
        if (!IsEditingGroup) return true;

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
}
