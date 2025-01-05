using System.Numerics;
using ImGuiNET;
using KogamaTools.Config;

namespace KogamaTools.GUI.Menus;

internal static class ConfigMenu
{
    private const float ButtonHeight = 19f;
    private static readonly Vector4 HighlightColor = new(1, 1, 0, 1);

    private static readonly IEnumerable<IGrouping<string, IGrouping<string, AutoConfigManager.BindEntry>>> GroupedEntries =
        KogamaTools.ConfigManager.BindEntries
            .Values
            .GroupBy(entry => entry.Entry.Definition.Section)
            .SelectMany(sectionGroup => sectionGroup
                .GroupBy(entry => KogamaTools.ConfigManager.BindEntries.First(kv => kv.Value == entry).Key.DeclaringType?.Name ?? "Unknown")
                .GroupBy(classGroup => sectionGroup.Key));

    internal static void Render()
    {
        if (!ImGui.BeginTabItem("Config"))
            return;

        ImGui.TextColored(HighlightColor, "This preset will be loaded on plugin startup.");

        if (ImGui.Button("Reset to defaults", new Vector2(ImGui.GetContentRegionAvail().X, ImGui.GetStyle().FrameBorderSize)))
        {
            ResetToDefaults();
        }

        foreach (var sectionGroup in GroupedEntries)
        {
            RenderSection(sectionGroup);
        }

        ImGui.EndTabItem();
    }

    private static void RenderSection(IGrouping<string, IGrouping<string, AutoConfigManager.BindEntry>> sectionGroup)
    {
        if (!ImGui.TreeNode(sectionGroup.Key))
            return;

        foreach (var classGroup in sectionGroup)
        {
            RenderClassGroup(classGroup);
        }

        ImGui.TreePop();
    }

    private static void RenderClassGroup(IGrouping<string, AutoConfigManager.BindEntry> classGroup)
    {
        if (!ImGui.TreeNode(classGroup.Key))
            return;

        foreach (var entry in classGroup)
        {
            RenderEntry(entry);
        }

        ImGui.TreePop();
    }

    private static void RenderEntry(AutoConfigManager.BindEntry bindEntry)
    {
        object currentValue = bindEntry.Entry.BoxedValue;

        if (GUIUtils.RenderControlForObject(bindEntry.Entry.Definition.Key, ref currentValue))
        {
            bindEntry.Entry.BoxedValue = currentValue;
        }
    }

    private static void ResetToDefaults()
    {
        foreach (var entry in KogamaTools.ConfigManager.BindEntries.Values)
        {
            entry.Entry.BoxedValue = entry.Entry.DefaultValue;
        }
    }
}
