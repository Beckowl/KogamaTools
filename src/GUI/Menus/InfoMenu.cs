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

        AddTableRow("World objects", GameInfo.WorldObjectCount);
        AddTableRow("Logic objects", GameInfo.LogicObjectCount);
        AddTableRow("Links", GameInfo.LinkCount);
        AddTableRow("Object links", GameInfo.ObjectLinkCount);
        AddTableRow("Models (unique)", GameInfo.UniquePrototypeCount);
        AddTableRow("Models (all)", GameInfo.PrototypeCount);

        ImGui.EndTable();

        if (ImGui.Button("Toggle Console"))
        {
            ConsoleToggle.ToggleConsole();
        }

        ImGui.Columns(2, "InfoTextColumn", false);
        ImGui.SetColumnOffset(1, ImGui.CalcTextSize("KoGaMa version:\t").X);

        ImGui.Text("Ping:");
        ImGui.Text("FPS:");
        ImGui.Text("KoGaMa version:");

        ImGui.NextColumn();

        ImGui.Text($"{GameInfo.Ping}ms");
        ImGui.Text($"{GameInfo.Fps}");
        ImGui.Text($"{GameInfo.GameVersion}");

        ImGui.Columns(1);

        ImGui.Text("\nMade by Becko.");
        ImGui.Text("\nSpecial thanks to MauryDev, Eveldee & Wowizowi.");

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