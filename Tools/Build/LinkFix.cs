using HarmonyLib;
using MV.WorldObject;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]

internal static class LinkFix
{
    internal static bool Enabled = false;
    private static bool connectorSelected = false;
    private static Link tempLink = new();

    [HarmonyPatch(typeof(FSMEntity), "PushState", new Type[] { typeof(EditorEvent) })]
    [HarmonyPrefix]
    private static bool PushState(EditorEvent nextState)
    {
        return !Enabled || nextState != EditorEvent.ESAddLink;
    }

    [HarmonyPatch(typeof(MVWorldObjectClient), "OnClickHandler")]
    [HarmonyPostfix]
    private static void OnClickHandler(ref MVWorldObjectClient __instance, bool __result)
    {
        if (!Enabled || !__result || !IsConnectorValid(__instance))
        {
            return;
        }

        if (!connectorSelected)
        {
            BeginLink(__instance);
        }
        else
        {
            EndLink(__instance);
        }
    }

    private static bool IsConnectorValid(MVWorldObjectClient wo)
    {
        return wo.SelectedConnector == SelectedConnector.Input || wo.SelectedConnector == SelectedConnector.Output;
    }

    private static void SetConnections(MVWorldObjectClient wo)
    {
        switch (wo.SelectedConnector)
        {
            case SelectedConnector.Input:
                tempLink.inputWOID = wo.id;
                break;
            case SelectedConnector.Output:
                tempLink.outputWOID = wo.id;
                break;
        }
    }

    private static void BeginLink(MVWorldObjectClient wo)
    {
        connectorSelected = true;
        tempLink = new Link();

        SetConnections(wo);

        MVGameControllerBase.MainCameraManager.LineDrawManager.SetTempLink(tempLink);
    }

    private static void EndLink(MVWorldObjectClient wo)
    {
        SetConnections(wo);

        if (tempLink.inputWOID != -1 && tempLink.outputWOID != -1)
        {
            MVGameControllerBase.OperationRequests.AddLink(tempLink);
        }

        MVGameControllerBase.MainCameraManager.LineDrawManager.SetTempLink(null);
        connectorSelected = false;
    }
}
