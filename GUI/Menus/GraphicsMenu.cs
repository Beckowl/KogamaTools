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

        if (ImGui.Checkbox("Display game UI", ref ToggleUI.UIVisible))
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => ToggleUI.UpdateUIVisibility());
        }
        if (ImGui.Checkbox("Fog enabled", ref FogModifier.FogEnabled))
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => FogModifier.ApplyChanges());
        }

        if (FogModifier.FogEnabled)
        {
            ImGui.SameLine();
            ImGui.Checkbox("Custom fog density", ref FogModifier.UseCustomFogDensity);

            if (FogModifier.UseCustomFogDensity)
            {
                if (ImGui.SliderFloat("Fog density", ref FogModifier.FogDensity, 0.0f, 0.05f))
                {
                    UnityMainThreadDispatcher.Instance.Enqueue(() => FogModifier.ApplyChanges());
                }
            }
        }

        if (ImGui.Checkbox("Reflective water", ref WaterReflectionModifier.UseReflectiveWater))
        {
            WaterReflectionModifier.ApplyChanges();
        }


        ImGui.Text("Resolution");

        ImGui.SameLine();
        ImGui.SetNextItemWidth(ImGui.CalcItemWidth() / 2);
        ImGui.PushID("WindowWidth");
        ImGui.InputInt(string.Empty, ref WindowModifier.Width);
        ImGui.PopID();

        ImGui.SameLine();
        ImGui.Text("x");

        ImGui.SetNextItemWidth(ImGui.CalcItemWidth() / 2);
        ImGui.SameLine();
        ImGui.PushID("WindowHeight");
        ImGui.InputInt(string.Empty, ref WindowModifier.Height);
        ImGui.PopID();

        ImGui.Text("Window mode");

        ImGui.SameLine();
        ImGui.SetNextItemWidth(-(GUIUtils.CalcButtonSize("Apply") + ImGui.GetStyle().ItemSpacing + ImGui.GetStyle().ItemInnerSpacing).X);
        ImGui.PushID("WindowMode");
        if (ImGui.BeginCombo(string.Empty, ((FullScreenMode)WindowModifier.SelectedFullScreenMode).ToString()))
        {
            for (int i = 0; i < WindowModifier.FullScreenModes.Count(); i++)
            {
                bool selected = WindowModifier.SelectedFullScreenMode == i;
                if (ImGui.Selectable(WindowModifier.FullScreenModes[i], ref selected))
                {
                    WindowModifier.SelectedFullScreenMode = i;
                }

                if (selected)
                {
                    ImGui.SetItemDefaultFocus();
                }
            }
            ImGui.EndCombo();
        }

        ImGui.PopID();

        ImGui.SameLine();
        if (ImGui.Button("Apply"))
        {
            WindowModifier.ApplyResolution();
        }


        if (GUIUtils.InputFloat("Shadow distance", ref ShadowDistModifier.ShadowDistance))
        {
            ShadowDistModifier.ApplyChanges();
        }


        if (GUIUtils.InputFloat("Draw Distance", ref ClipPlaneModifier.FarClipPlane))
        {
            ClipPlaneModifier.ApplyChanges();
        }

        if (ImGui.Checkbox("Ortographic camera", ref OrtographicCamera.Enabled))
        {
            OrtographicCamera.ApplyChanges();
        }

        if (OrtographicCamera.Enabled)
        {
            if (GUIUtils.InputFloat("Ortographic camera size", ref OrtographicCamera.Size))
            {
                OrtographicCamera.ApplyChanges();
            }
        }

        if (ImGui.Checkbox("Themes enabled", ref ThemeModifier.ThemesEnabled))
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() =>
              ThemeModifier.ApplyToggleThemes());
        }

        if (ThemeModifier.ThemesEnabled)
        {

            ImGui.SetNextItemWidth(-(ImGui.CalcTextSize("Theme preview") + GUIUtils.CalcButtonSize("Create") + GUIUtils.CalcButtonSize("Destroy") + (ImGui.GetStyle().ItemSpacing + ImGui.GetStyle().ItemInnerSpacing) * 2).X);
            if (ImGui.BeginCombo("Theme preview", ThemeModifier.ThemeIDs[ThemeModifier.SelectedThemePreview]))
            {
                for (int i = 0; i < ThemeModifier.ThemeIDs.Length; i++)
                {
                    bool selected = ThemeModifier.SelectedThemePreview == i;
                    if (ImGui.Selectable(ThemeModifier.ThemeIDs[i]))
                    {
                        ThemeModifier.SelectedThemePreview = i;
                    }

                    if (selected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }
                ImGui.EndCombo();
            }

            ImGui.SameLine();
            if (ImGui.Button("Create"))
            {
                UnityMainThreadDispatcher.Instance.Enqueue(() =>
                  ThemeModifier.CreateThemePreview());
            }

            ImGui.SameLine();
            if (ImGui.Button("Destroy"))
            {
                UnityMainThreadDispatcher.Instance.Enqueue(() =>
                  ThemeModifier.DestroyThemePreview());
            }
        }

        ImGui.EndTabItem();
    }

}