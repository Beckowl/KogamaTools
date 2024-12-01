using System.Text;
using ImGuiNET;
using KogamaTools.Behaviours;
using KogamaTools.Helpers;
using KogamaTools.Tools.PVP;

namespace KogamaTools.GUI.Menus;

internal static class PVPMenu
{
    private static byte[] customCrosshairPath = new byte[1024];

    private static bool antiAFKEnabled = false;
    internal static void Render()
    {

        if (MVGameControllerBase.GameMode == MV.Common.MVGameMode.CharacterEditor)
            return;

        if (!ImGui.BeginTabItem("PvP"))
            return;


        ImGui.Checkbox("Fast respawn", ref FastRespawn.Enabled);

        if (ImGui.Checkbox("Anti AFK", ref antiAFKEnabled))
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                AwayMonitor.instance.idleKickEnabled = antiAFKEnabled;
                AwayMonitor.IdleKickEnabled = antiAFKEnabled;
            });
        }

        ImGui.Checkbox("Camera Focus", ref FOVModifier.FocusSettings.CameraFocusEnabled);

        if (FOVModifier.FocusSettings.CameraFocusEnabled)
        {
            ImGui.SameLine();
            ImGui.Checkbox("Override rail gun zoom", ref FOVModifier.FocusSettings.OverrideRailGunZoom);

            GUIUtils.InputFloat("FOV multiplier", ref FOVModifier.FocusSettings.FOVMultiplier);
            GUIUtils.InputFloat("Sensitivity multiplier", ref FOVModifier.FocusSettings.SensitivityMultiplier);
            GUIUtils.InputFloat("Zoom speed", ref FOVModifier.FocusSettings.FocusSpeed);
        }

        ImGui.Checkbox("Custom FOV", ref FOVModifier.CustomFOVEnabled);

        if (FOVModifier.CustomFOVEnabled)
        {
            GUIUtils.InputFloat("FOV", ref FOVModifier.CustomFOV);
        }

        ImGui.Checkbox("Custom crosshair color", ref CustomCrossHairColor.Enabled);

        if (CustomCrossHairColor.Enabled)
        {
            System.Numerics.Vector4 crosshaircolor = ColorHelper.ToVector4(CustomCrossHairColor.Color);
            if (ImGui.ColorEdit4("Crosshair color", ref crosshaircolor))
            {
                CustomCrossHairColor.SetCrossHairColorFromVec4(crosshaircolor);
            }
        }
        ImGui.SetNextItemWidth(-(GUIUtils.CalcButtonSize("Load") + GUIUtils.CalcButtonSize("Clear") + ImGui.CalcTextSize("Custom crosshair") + ImGui.GetStyle().ItemSpacing*2 + ImGui.GetStyle().ItemInnerSpacing).X);

        ImGui.InputText("Custom crosshair", customCrosshairPath, (uint)customCrosshairPath.Length);


        ImGui.SameLine();

        if (ImGui.Button("Load"))
        {
            string path = Encoding.UTF8.GetString(customCrosshairPath).TrimEnd('\0');
            UnityMainThreadDispatcher.Instance.Enqueue(() => CustomCrossHairColor.SetCrossHairTexture(path));

        }

        ImGui.SameLine();

        if (ImGui.Button("Clear"))
        {
            customCrosshairPath = new byte[1024];
        }

        ImGui.EndTabItem();
    }
}
