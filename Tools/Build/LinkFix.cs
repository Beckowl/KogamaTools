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
        Vector3 mousePosition = MVInputWrapper.GetPointerPosition();

        if (wo.HasInputConnector && wo.IsPointOverInputConnector(mousePosition))
        {
            connector = SelectedConnector.Input;
            return true;
        }

        if (wo.HasOutputConnector && wo.IsPointOverOutputConnector(mousePosition))
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

    [HarmonyPatch(typeof(ESAddObjectLink), "Enter")]
    [HarmonyPrefix]
    private static bool Enter(ESAddObjectLink __instance, EditorStateMachine esm)
    {
        if (!Enabled) return true;

        MVWorldObjectClient worldObject = null!;

        VoxelHit voxelHit = new();

        if (ObjectPicker.Pick(ref voxelHit) && voxelHit.woId != -1)
        {
            worldObject = MVGameControllerBase.WOCM.GetWorldObjectClient(voxelHit.woId);
        }

        if (worldObject != null)
        {
            __instance.tempLink = new ObjectLink();
            if (worldObject.SelectedConnector != SelectedConnector.Object)
            {
                Debug.LogError("Should not happen - object links can only be added when starting from object-connector");
                esm.PopState();
                return false;
            }
            __instance.tempLink.objectConnectorWOID = worldObject.Id;
            MVGameControllerBase.MainCameraManager.LineDrawManager.SetTempObjectLink(__instance.tempLink);
            __instance.woRef = MVGameControllerBase.WOCM.GetWorldObjectClientRef(worldObject.Id);
        }
        return false;
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