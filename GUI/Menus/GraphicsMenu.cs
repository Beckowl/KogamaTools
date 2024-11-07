using ImGuiNET;
using KogamaTools.Behaviours;
using KogamaTools.Tools.Graphics;
using UnityEngine;

namespace KogamaTools.GUI.Menus;
internal class GraphicsMenu
{
    internal static void Render()
    {
        if (!ImGui.BeginTabItem("Graphics"))
            return;

        if (ImGui.Checkbox("Fog enabled", ref FogModifier.FogEnabled))
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => { RenderSettings.fog = FogModifier.FogEnabled; });
        }
        if (FogModifier.FogEnabled)
        {
            if (ImGui.SliderFloat("Fog density", ref FogModifier.FogDensity, 0, 1)) 
            {
                UnityMainThreadDispatcher.Instance.Enqueue(() => { RenderSettings.fogDensity = FogModifier.FogDensity; });
            }
            ImGui.SameLine();

            if (ImGui.InputFloat("", ref FogModifier.FogDensity))
            {
                UnityMainThreadDispatcher.Instance.Enqueue(() => { RenderSettings.fogDensity = FogModifier.FogDensity; });
            }    
        }

        ImGui.InputInt2("Resolution", ref ResolutionModifier.resolution[0]);
        ImGui.Checkbox("Fullscreen", ref ResolutionModifier.fullscreen);
        ImGui.SameLine();
        if (ImGui.Button("Apply"))
        {
            ResolutionModifier.ApplyResolution();
        }

        if (ImGui.InputFloat("Shadow distance", ref ShadowDistModifier.ShadowDistance))
        {
            ShadowDistModifier.ApplyShadowDistance();
        }

        if (ImGui.InputFloat("Draw Distance", ref ClipPlaneModifier.FarClipPlane))
        {
            ClipPlaneModifier.ApplyClipPlane();
        }

        if (ImGui.Checkbox("Themes enabled", ref ThemeModifier.ThemesEnabled))
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                ThemeModifier.UpdateThemesEnabled();
            });
        }

        ImGui.EndTabItem();
    }

}
