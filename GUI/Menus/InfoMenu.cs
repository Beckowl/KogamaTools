using ImGuiNET;
using KogamaTools.Tools.Misc;

namespace KogamaTools.GUI.Menus;
internal class InfoMenu
{
    internal static void Render()
    {
        if (!ImGui.BeginTabItem("Info")) return;

        if (!ImGui.BeginTable("Table", 2, ImGuiTableFlags.Borders)) return;

        ImGui.TableSetupColumn("Item");
        ImGui.TableSetupColumn("Count");
        ImGui.TableHeadersRow();

        AddTableRow("World objects", GameMetrics.WorldObjectCount);
        AddTableRow("Logic objects", GameMetrics.LogicObjectCount);
        AddTableRow("Links", GameMetrics.LinkCount);
        AddTableRow("Object links", GameMetrics.ObjectLinkCount);
        AddTableRow("Models (unique)", GameMetrics.UniquePrototypeCount);
        AddTableRow("Models (all)", GameMetrics.PrototypeCount);

        ImGui.EndTable();

        ImGui.Text("\n");
        ImGui.Text($"Ping:\t{GameMetrics.Ping}ms");
        ImGui.Text($"FPS:\t{GameMetrics.Fps}");
        ImGui.Text("\nMade by Becko.");
        ImGui.Text("\nSpecial thanks to MauryDev & Eveldee");

        ImGui.EndTabItem();
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