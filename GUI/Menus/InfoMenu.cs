using ImGuiNET;
using KogamaTools.Behaviours;
using UnityEngine;

namespace KogamaTools.GUI.Menus;

internal class InfoMenu
{
    private static int worldObjectCount;
    private static int linkCount;
    private static int objectLinkCount;
    private static int uniquePrototypeCount;
    private static int prototypeCount;
    private static int ping;
    private static float fps;

    internal static void Render()
    {
        UnityMainThreadDispatcher.Instance.Enqueue(UpdateMetrics);

        if (ImGui.BeginTabItem("Info"))
        {
            if (ImGui.BeginTable("Table", 2, ImGuiTableFlags.Borders))
            {
                ImGui.TableSetupColumn("Item");
                ImGui.TableSetupColumn("Count");
                ImGui.TableHeadersRow();

                AddTableRow("World objects", worldObjectCount);
                AddTableRow("Links", linkCount);
                AddTableRow("Object links", objectLinkCount);
                AddTableRow("Models (unique)", uniquePrototypeCount);
                AddTableRow("Models (all)", prototypeCount);

                ImGui.EndTable();
            }

            ImGui.Text("\n");
            ImGui.Text($"Ping:\t{ping}ms");
            ImGui.Text($"FPS:\t{fps}");
            ImGui.Text("\nMade by Becko.");

            ImGui.EndTabItem();
        }
    }

    private static void UpdateMetrics()
    {
        if (!MVGameControllerBase.IsInitialized) return;

        worldObjectCount = MVGameControllerBase.WOCM.worldObjects.Count;
        linkCount = MVGameControllerBase.Game.worldNetwork.links.links.Count;
        objectLinkCount = MVGameControllerBase.Game.worldNetwork.objectLinks.objectLinks.Count;
        uniquePrototypeCount = MVGameControllerBase.Game.worldNetwork.worldInventory.runtimePrototypes.Count;
        prototypeCount = GetPrototypeCount();
        ping = MVGameControllerBase.Game.Peer.RoundTripTime;
        fps = 1 / Time.deltaTime;

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

    private static void AddTableRow(string description, int count)
    {
        ImGui.TableNextRow();
        ImGui.TableSetColumnIndex(0);
        ImGui.Text(description);
        ImGui.TableSetColumnIndex(1);
        ImGui.Text(count.ToString());
    }
}
