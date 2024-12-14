using System.Numerics;
using ImGuiNET;
using KogamaTools.Helpers;

namespace KogamaTools.GUI;

internal static class GUIUtils
{
    private const int maxStringLength = 512;

    internal static Vector2 CalcButtonSize(string label)
    {
        return ImGui.CalcTextSize(label) + ImGui.GetStyle().FramePadding * 2;
    }

    internal static Vector2 CalcTextSize(params string[] text)
    {
        Vector2 result = new();

        foreach (string s in text)
        {
            result += ImGui.CalcTextSize(s);
        }

        return result;
    }

    internal static Vector2 CalcSpacing(int numItems)
    {
        var style = ImGui.GetStyle();
        return (style.ItemSpacing*2) + style.ItemInnerSpacing * (numItems-1);
    }

    internal static bool InputFloat(string label, ref float value)
    {
        ImGui.PushID(label);
        ImGui.Text(label);
        ImGui.SameLine();
        ImGui.SetNextItemWidth((ImGui.GetContentRegionAvail() - ImGui.GetStyle().FramePadding).X);
        bool result = ImGui.InputFloat(string.Empty, ref value);
        ImGui.PopID();
        return result;
    }
    /*
    private static bool RenderColor(string label, ref object value)
    {
        Vector4 temp = ColorHelper.ToVector4((UnityEngine.Color)value);
        if (ImGui.ColorEdit4(label, ref temp))
        {
            value = ColorHelper.ToUnityColor(temp);
            return true;
        }
        return false;
    }
    */
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
