using HarmonyLib;
using MV.WorldObject;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal class GroupEdit : MonoBehaviour
{
    internal static bool Enabled = false;

    [HarmonyPatch(typeof(ContextMenuController), "PopGizmos")]
    [HarmonyPrefix]
    private static void PopGizmos(ContextMenuController __instance)
    {
        if (!Enabled) return;

        var menus = FindObjectsOfType<ContextMenu>();

        ContextMenu? menu = menus.FirstOrDefault();

        if (menu == null) return;

        if (MVGameControllerBase.WOCM.IsType(__instance.woID, WorldObjectType.Group))
        {
            menu.AddButton("Edit Group", new Action(__instance.EnterCubeEdit));
        }
    }

    [HarmonyPatch(typeof(MVGroup), "OnEnterObject")]
    [HarmonyPostfix]
    private static void OnEnterObject(MVGroup __instance)
    {
        HighLightLinks(__instance, true);
    }

    private static void HighLightLinks(MVWorldObjectClient wo, bool highlight)
    {
        MVGroup group = wo.Cast<MVGroup>();

        foreach (MVWorldObjectClient mvworldObjectClient in group.children.Values)
        {
            foreach (Link link in mvworldObjectClient.OutputLinkRefs)
            {
                LinkObjectScript linkscript = MVGameControllerBase.Game.worldNetwork.links.linkObjects[link.id];

                SharedCubeFunctions.SetLayerRecursively(linkscript.transform, highlight);
            }
            SharedCubeFunctions.SetLayerRecursively(wo.transform, highlight);
        }
    }

    [HarmonyPatch(typeof(DesktopEditModeController), "EnterPlayMode")]
    [HarmonyPrefix]
    private static void EnterPlayMode()
    {
        MVGroup parentGroup = MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>().EditModeStateMachine.ParentGroup;

        HighLightLinks(parentGroup, false);
    }

    [HarmonyPatch(typeof(ESTranslate), "Exit")]
    [HarmonyPrefix]
    private static bool Exit(EditorStateMachine e, ESTranslate __instance)
    {
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
