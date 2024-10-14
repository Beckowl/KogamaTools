using HarmonyLib;
using KogamaTools.Helpers;
using MV.WorldObject;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal class LinkFix : MonoBehaviour
{
    internal static bool Enabled = true;
    private static Link tempLink = new();
    private static int connectorCounter = 0;

    private void Update()
    {
        if (!MVGameControllerBase.IsInitialized) return;
        if (!Enabled || EventSystem.current.IsPointerOverGameObject() || MVGameControllerBase.Game.IsPlaying) return;

        HandleLinkContextMenu();
        HandleConnectors();
    }

    private static void HandleLinkContextMenu()
    {
        if (MVInputWrapper.GetBooleanControlDown(KogamaControls.PointerSelectAlt))
        {
            VoxelHit voxelHit = new();
            var pickedLink = ObjectPicker.PickLink(ref voxelHit);

            if (pickedLink != null)
            {
                MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>().contextMenuController.ShowContextMenuLink(pickedLink.linkID, pickedLink.isObjectLink, voxelHit.point);
            }
        }
    }

    private static void HandleConnectors()
    {
        if (MVInputWrapper.GetBooleanControlDown(KogamaControls.PointerSelect))
        {
            VoxelHit voxelHit = new();

            if (ObjectPicker.Pick(ref voxelHit) && voxelHit.woId != -1)
            {
                MVWorldObjectClient worldObjectClient = MVGameControllerBase.WOCM.GetWorldObjectClient(voxelHit.woId);
                if (worldObjectClient != null && IsPointerOverConnector(worldObjectClient, out SelectedConnector connector))
                {
                    HandleAddLink(worldObjectClient, connector);
                    return;
                }
            }

            ResetTempLink();
        }
    }

    private static bool IsPointerOverConnector(MVWorldObjectClient wo, out SelectedConnector connector)
    {
        connector = SelectedConnector.None;

        if (wo.HasInputConnector && wo.IsPointOverInputConnector(MVInputWrapper.GetPointerPosition()))
        {
            connector = SelectedConnector.Input;
            return true;
        }
        else if (wo.HasOutputConnector && wo.IsPointOverOutputConnector(MVInputWrapper.GetPointerPosition()))
        {
            connector = SelectedConnector.Output;
            return true;
        }
        return false;
    }

    private static void HandleAddLink(MVWorldObjectClient wo, SelectedConnector connectorType)
    {
        switch (connectorType)
        {
            case SelectedConnector.Input:
                tempLink.inputWOID = wo.id;
                break;
            case SelectedConnector.Output:
                tempLink.outputWOID = wo.id;
                break;
        }

        connectorCounter++;

        MVGameControllerBase.MainCameraManager.lineDrawManager.SetTempLink(tempLink);

        if (connectorCounter >= 2)
        {
            if (tempLink.inputWOID != -1 && tempLink.outputWOID != -1)
            {
                MVGameControllerBase.OperationRequests.AddLink(tempLink);
            }
            ResetTempLink();
        }
    }

    private static void ResetTempLink()
    {
        MVGameControllerBase.MainCameraManager.LineDrawManager.SetTempLink(null);
        tempLink = new();
        connectorCounter = 0;
    }

    [HarmonyPatch(typeof(FSMEntity), "PushState", new Type[] { typeof(EditorEvent) })]
    [HarmonyPrefix]
    private static bool PushState(EditorEvent nextState)
    {
        return !Enabled || nextState != EditorEvent.ESAddLink;
    }

    [HarmonyPatch(typeof(DesktopEditModeController), "EnterPlayMode")]
    [HarmonyPrefix]
    private static void EnterPlayMode()
    {
        ResetTempLink();
    }
}