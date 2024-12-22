using ImGuiNET;
using KogamaTools.Behaviours;
using KogamaTools.Tools.Graphics;

namespace KogamaTools.GUI.Menus;
internal class GraphicsMenu
{
    internal static void Render()
    {
        if (!ImGui.BeginTabItem("Graphics"))
            return;

        if (ImGui.Checkbox("Show game UI", ref UIToggle.UIVisible))
        {
            UnityMainThreadDispatcher.Instance.Enqueue(UIToggle.UpdateUIVisibility);
        }

        if (ImGui.Checkbox("Show chat", ref ChatToggle.ChatVisible))
        {
            UnityMainThreadDispatcher.Instance.Enqueue(ChatToggle.UpdateChatVisibility);
        }

        ImGui.Checkbox("Show notifications", ref NotificationToggle.ShowNotifications);

        if (ImGui.Checkbox("Fog enabled", ref FogModifier.FogEnabled))
        {
            UnityMainThreadDispatcher.Instance.Enqueue(FogModifier.ApplyChanges);
        }

        if (FogModifier.FogEnabled)
        {
            ImGui.SameLine();

            if (ImGui.Checkbox("Custom fog density", ref FogModifier.UseCustomFogDensity))
            {
                UnityMainThreadDispatcher.Instance.Enqueue(FogModifier.ApplyChanges);
            }

            if (FogModifier.UseCustomFogDensity)
            {
                if (ImGui.SliderFloat("Fog density", ref FogModifier.FogDensity, 0.0f, 0.05f))
                {
                    UnityMainThreadDispatcher.Instance.Enqueue(FogModifier.ApplyChanges);
                }
            }
        }

        if (ImGui.Checkbox("Reflective water", ref WaterReflectionModifier.UseReflectiveWater))
        {
            WaterReflectionModifier.ApplyChanges();
        }

        ImGui.Text("Resolution");

        ImGui.SameLine();

        float inputWidth = (ImGui.GetContentRegionAvail() / 2 - ImGui.CalcTextSize("x") - ImGui.GetStyle().FramePadding).X - 2;

        ImGui.SetNextItemWidth(inputWidth);
        ImGui.InputInt("##windowWidth", ref WindowModifier.Width);

        ImGui.SameLine();
        ImGui.Text("x");

        ImGui.SameLine();
        ImGui.SetNextItemWidth(inputWidth);
        ImGui.InputInt("##windowHeight", ref WindowModifier.Height);

        ImGui.Text("Window mode");

        ImGui.SameLine();
        ImGui.SetNextItemWidth((ImGui.GetContentRegionAvail() - GUIUtils.CalcButtonSize("Apply") - ImGui.GetStyle().ItemSpacing - ImGui.GetStyle().FramePadding).X);
        GUIUtils.RenderEnum("##windowMode", ref WindowModifier.screenMode);

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

        if (GUIUtils.InputFloat("Camera distance to avatar", ref CameraDistanceModifier.distance))
        {
            CameraDistanceModifier.ApplyChanges();
        }

        ImGui.Checkbox("Apply custom FOV globally", ref FOVModifier.ApplyGlobally);

        if (FOVModifier.ApplyGlobally)
        {
            GUIUtils.InputFloat("FOV", ref FOVModifier.CustomFOV);
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
            UnityMainThreadDispatcher.Instance.Enqueue(ThemeModifier.ApplyToggleThemes);
        }

        if (ThemeModifier.ThemesEnabled)
        {
            ImGui.SetNextItemWidth(-(GUIUtils.CalcTextSize("Theme preview") + GUIUtils.CalcButtonSize("Create") + GUIUtils.CalcButtonSize("Destroy") + GUIUtils.CalcSpacing(2) + ImGui.GetStyle().FramePadding).X);
            GUIUtils.RenderEnum("Theme preview", ref ThemeModifier.SelectedTheme);

            ImGui.SameLine();
            if (ImGui.Button("Create"))
            {
                UnityMainThreadDispatcher.Instance.Enqueue(ThemeModifier.CreateThemePreview);
            }

            ImGui.SameLine();
            if (ImGui.Button("Destroy"))
            {
                UnityMainThreadDispatcher.Instance.Enqueue(ThemeModifier.DestroyThemePreview);
            }
        }

        if (ImGui.Button("Capture screenshot (F2)"))
        {
            UnityMainThreadDispatcher.Instance.Enqueue(ScreenshotUtil.CaptureScreenshot);
        }

        ImGui.SameLine();
        GUIUtils.InputFloat("Super size", ref ScreenshotUtil.SuperSize);

        ImGui.EndTabItem();
    }

}