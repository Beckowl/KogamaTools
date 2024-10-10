using HarmonyLib;
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
            obj => MVGameControllerBase.WOCM.IsType(obj.id, WorldObjectType.Group) && !GroupStack.Contains(obj),
            "Edit Group",
            (menu) => EnterGroupEdit(menu.selectedWorldObject)
        );
    }

    private static void EnterGroupEdit(MVWorldObjectClient wo)
    {
        if (wo.id == MVGameControllerBase.WOCM.RootGroup.Id) return;

        EditorStateMachine esm = MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>().EditModeStateMachine;

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
        EditorStateMachine esm = MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>().EditModeStateMachine;

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
}
