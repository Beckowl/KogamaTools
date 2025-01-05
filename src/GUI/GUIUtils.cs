using ImGuiNET;

namespace KogamaTools.GUI;

internal static class GUIUtils
{
    public static string RemoveIdentifier(string label)
    {
        int separatorIndex = label.IndexOf("##", StringComparison.Ordinal);

        if (separatorIndex == -1)
        {
            return label;
        }

        return label.Substring(0, separatorIndex);
    }

    internal static float CalcButtonWidth(string label)
    {
        return (ImGui.CalcTextSize(RemoveIdentifier(label)) + ImGui.GetStyle().FramePadding * 2).X;
    }

    internal static float CalcLabelWidth(string label)
    {
        return ImGui.CalcTextSize(RemoveIdentifier(label)).X + ImGui.GetStyle().FramePadding.X;
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
        ImGui.Text(RemoveIdentifier(label));
        ImGui.SameLine();
        ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X); // floats like cutting their fields/labels off for some reason
        bool result = ImGui.InputFloat(string.Empty, ref value);
        ImGui.PopID();
        return result;
    }

    internal static bool RenderControlForObject(string label, ref object value)
    {
        bool modified = false;

        switch (value)
        {
            case bool boolValue:
                modified = RenderBool(label, ref boolValue);
                value = boolValue;
                break;

            case int intValue:
                modified = RenderInt(label, ref intValue);
                value = intValue;
                break;

            case float floatValue:
                modified = RenderFloat(label, ref floatValue);
                value = floatValue;
                break;

            case string stringValue:
                modified = RenderString(label, ref stringValue);
                value = stringValue;
                break;

            default:
                ImGui.Text($"Unsupported type for {label}");
                break;
        }

        return modified;
    }

    private static bool RenderBool(string label, ref bool value)
    {
        return ImGui.Checkbox(label, ref value);
    }

    private static bool RenderInt(string label, ref int value)
    {
        int temp = value;
        bool modified = ImGui.DragInt(label, ref temp);
        if (modified) value = temp;
        return modified;
    }

    private static bool RenderFloat(string label, ref float value)
    {
        float temp = value;
        bool modified = InputFloat(label, ref temp);
        if (modified) value = temp;
        return modified;
    }

    private static bool RenderString(string label, ref string value)
    {
        ImGui.SetNextItemWidth(-CalcReservedButtonSpaceLabel(label)); 
        return ImGui.InputText(label, ref value, 1024);
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
