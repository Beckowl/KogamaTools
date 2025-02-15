﻿using UnityEngine;

namespace KogamaTools.Tools.Misc;
internal static class GameInfo
{
    internal static int WorldObjectCount;
    internal static int LogicObjectCount;
    internal static int LinkCount;
    internal static int ObjectLinkCount;
    internal static int UniquePrototypeCount;
    internal static int PrototypeCount;
    internal static int Ping;
    internal static float Fps;
    internal static string GameVersion = MVGameControllerBase.KoGaMaSettings.VersionString;

    internal static void UpdateMetrics()
    {
        WorldObjectCount = MVGameControllerBase.WOCM.worldObjects.Count;
        LogicObjectCount = GetLogicObjectCount();
        LinkCount = MVGameControllerBase.Game.worldNetwork.links.links.Count;
        ObjectLinkCount = MVGameControllerBase.Game.worldNetwork.objectLinks.objectLinks.Count;
        UniquePrototypeCount = MVGameControllerBase.Game.worldNetwork.worldInventory.runtimePrototypes.Count;
        PrototypeCount = GetPrototypeCount();
        Ping = MVGameControllerBase.Game.Peer.RoundTripTime;
        Fps = 1 / Time.smoothDeltaTime;
    }

    private static int GetPrototypeCount()
    {
        int count = 0;

        foreach (MVWorldObjectClient wo in MVGameControllerBase.WOCM.worldObjects.Values)
        {
            if (wo.HasInteractionFlag(InteractionFlags.HasCubeModel))
            {
                count++;
            }
        }

        return count;
    }

    private static int GetLogicObjectCount()
    {
        int count = 0;

        foreach (MVWorldObjectClient wo in MVGameControllerBase.WOCM.worldObjects.Values)
        {
            if (wo.HasInputConnector || wo.HasObjectConnector || wo.HasOutputConnector)
            {
                count++;
            }
        }

        return count;
    }
}

internal class GameMetricsUpdater : MonoBehaviour
{
    const float updateInterval = 1f / 10f;

    void Awake()
    {
        InvokeRepeating(nameof(UpdateMetrics), 0f, updateInterval);
    }

    private void UpdateMetrics()
    {
        GameInfo.UpdateMetrics();
    }
}
