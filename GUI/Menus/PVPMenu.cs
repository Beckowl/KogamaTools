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
        if (!ImGui.BeginTabItem("PvP"))
            return;

        ImGui.PushItemWidth(100);

        ImGui.Checkbox("Fast respawn", ref FastRespawn.Enabled);

        if (ImGui.Checkbox("Anti AFK", ref antiAFKEnabled))
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                AwayMonitor.instance.idleKickEnabled = antiAFKEnabled;
                AwayMonitor.IdleKickEnabled = antiAFKEnabled;
            });
        }

        ImGui.Checkbox("Camera Focus", ref CameraMod.FocusSettings.CameraFocusEnabled);

        if (CameraMod.FocusSettings.CameraFocusEnabled)
        {
            ImGui.SameLine();
            ImGui.Checkbox("Override rail gun zoom", ref CameraMod.FocusSettings.OverrideRailGunZoom);

            ImGui.InputFloat("FOV multiplier", ref CameraMod.FocusSettings.FOVMultiplier);
            ImGui.InputFloat("Sensitivity multiplier", ref CameraMod.FocusSettings.SensitivityMultiplier);
            ImGui.InputFloat("Zoom speed", ref CameraMod.FocusSettings.FocusSpeed);
        }

        ImGui.Checkbox("Custom FOV", ref CameraMod.CustomFOVEnabled);

        if (CameraMod.CustomFOVEnabled)
        {
            ImGui.InputFloat("FOV", ref CameraMod.CustomFOV);
        }

        ImGui.PopItemWidth();
        ImGui.PushItemWidth(160);

        ImGui.Checkbox("Custom crosshair color", ref CrossHairMod.CustomCrossHairColorEnabled);

        if (CrossHairMod.CustomCrossHairColorEnabled)
        {
            System.Numerics.Vector4 crosshaircolor = ColorHelper.ToVector4(CrossHairMod.CrossHairColor);
            if (ImGui.ColorEdit4("Crosshair color", ref crosshaircolor))
            {
                CrossHairMod.SetCrossHairColorFromVec4(crosshaircolor);
            }
        }

        ImGui.InputText("Custom crosshair", customCrosshairPath, (uint)customCrosshairPath.Length);

        ImGui.SameLine();

        if (ImGui.Button("Load"))
        {
            string path = Encoding.UTF8.GetString(customCrosshairPath).TrimEnd('\0');
            UnityMainThreadDispatcher.Instance.Enqueue(() => CrossHairMod.SetCrossHairTexture(path));

        }

        ImGui.SameLine();

        if (ImGui.Button("Clear"))
        {
            customCrosshairPath = new byte[1024];
        }

        ImGui.PopItemWidth();
        ImGui.EndTabItem();
    }
}
