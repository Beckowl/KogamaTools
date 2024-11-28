using UnityEngine;

namespace KogamaTools.Tools.Misc;
internal static class GameMetrics
{
    internal static int WorldObjectCount;
    internal static int LogicObjectCount;
    internal static int LinkCount;
    internal static int ObjectLinkCount;
    internal static int UniquePrototypeCount;
    internal static int PrototypeCount;
    internal static int Ping;
    internal static float Fps;

    internal static void UpdateMetrics()
    {
        WorldObjectCount = MVGameControllerBase.WOCM.worldObjects.Count;
        LogicObjectCount = MVGameControllerBase.Game.LogicObjectManager.logicWorldObjects.Count;
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
}

internal class GameMetricsUpdater : MonoBehaviour
{
    private void Update()
    {
        GameMetrics.UpdateMetrics();
    }
}
