using System.Numerics;
using ImGuiNET;
using KogamaTools.Helpers;

namespace KogamaTools.GUI;

internal static class GUIUtils
{
    private const int maxStringLength = 512;

    internal static bool RenderControlForObject(string label, ref object value)
    {
        bool result = value switch
        {
            UnityEngine.Color => RenderColor(label, ref value),
            int => RenderInt(label, ref value),
            string => RenderString(label, ref value),
            bool => RenderBool(label, ref value),
            float => RenderFloat(label, ref value),
            Enum => RenderEnum(label, ref value),
            _ => WarnInvalid(value)
        };

        return result;
    }

    private static bool WarnInvalid(object value)
    {
        //KogamaTools.mls.LogInfo($"Cannot render control for {value}");
        return false;
    }

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
    private static bool RenderInt(string label, ref object value)
    {
        int temp = (int)value;
        if (ImGui.InputInt(label, ref temp))
        {
            value = temp;
            return true;
        }
        return false;
    }

    private static bool RenderString(string label, ref object value)
    {
        string tmp = (string)value;
        if (ImGui.InputText(label, ref tmp, maxStringLength))
        {
            value = tmp;
            return true;
        }
        return false;
    }

    private static bool RenderBool(string label, ref object value)
    {
        bool tmp = (bool)value;
        if (ImGui.Checkbox(label, ref tmp))
        {
            value = tmp;
            return true;
        }
        return false;
    }

    private static bool RenderFloat(string label, ref object value)
    {
        float tmp = (float)value;
        if (ImGui.InputFloat(label, ref tmp))
        {
            value = tmp;
            return true;
        }
        return false;
    }

    private static bool RenderEnum(string label, ref object value)
    {
        int index = (int)value;
        string[] names = Enum.GetNames(value.GetType()).ToArray();

        if (ImGui.Combo(label, ref index, names, names.Length))
        {
            value = index;
            return true;
        }
        return false;
    }
}
