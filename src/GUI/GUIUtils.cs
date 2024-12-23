using System.Numerics;
using ImGuiNET;

namespace KogamaTools.GUI;

internal static class GUIUtils
{
    internal static float CalcButtonWidth(string label)
    {
        return (ImGui.CalcTextSize(label) + ImGui.GetStyle().FramePadding * 2).X;
    }

    internal static float CalcLabelWidth(string label)
    {
        return ImGui.CalcTextSize(label).X + ImGui.GetStyle().FramePadding.X;
    }

    internal static float CalcReservedButtonSpace(params string[] labels)
    {
        float itemSpacing = ImGui.GetStyle().ItemSpacing.X;
        float totalButtonWidth = 0f;

        foreach (string label in labels)
        {
            totalButtonWidth += CalcButtonWidth(label);
        }

        if (labels.Length > 0)
        {
            totalButtonWidth += itemSpacing * (labels.Length);
        }

        return totalButtonWidth;
    }

    internal static float CalcReservedButtonSpaceLabel(string currentLabel, params string[] labels)
    {
        return CalcReservedButtonSpace(labels) + CalcLabelWidth(currentLabel);
    }

    internal static float CalcSharedItemSpace(int numItems, float reservedSpace = 0)
    {
        if (numItems <= 0)
        {
            return 0f;
        }

        float contentWidth = ImGui.GetContentRegionAvail().X - reservedSpace;
        float itemSpacing = ImGui.GetStyle().ItemSpacing.X;

        return (contentWidth - itemSpacing * (numItems - 1)) / numItems;
    }

    internal static bool InputFloat(string label, ref float value)
    {
        ImGui.PushID(label);
        ImGui.Text(label);
        ImGui.SameLine();
        ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X); // floats like cutting their fields/labels off for some reason
        bool result = ImGui.InputFloat(string.Empty, ref value);
        ImGui.PopID();
        return result;
    }

    internal static bool RenderEnum<T>(string label, ref T value) where T : Enum
    {
        string[] names = Enum.GetNames(typeof(T)).ToArray();
        int index = Array.IndexOf(names, value.ToString());

        if (ImGui.Combo(label, ref index, names, names.Length))
        {
            value = (T)Enum.Parse(typeof(T), names[index]);
            return true;
        }
        return false;
    }
}
