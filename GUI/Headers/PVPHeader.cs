using System.Numerics;
using ImGuiNET;
using KogamaTools.Behaviours;
using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.GUI.Headers
{
    internal static class PVPHeader
    {
        private static bool antiAFKEnabled = ConfigHelper.GetConfigValue<bool>("AntiAFKEnabled");
        internal static void Render()
        {
            if (ImGui.CollapsingHeader("PvP"))
            {
                ImGui.PushItemWidth(100);

                ImGui.Checkbox("Fast respawn", ref FastRespawn.Enabled);

                if (ImGui.Checkbox("Anti AFK", ref antiAFKEnabled))
                {
                    KogamaTools.unityMainThreadDispatcher.Enqueue(() => AwayMonitor.instance.idleKickEnabled = antiAFKEnabled); // just disabling IdleKickEnabled didn't work for some reason
                }

                ImGui.Checkbox("Camera Focus", ref CameraFocus.Enabled);

                if (CameraFocus.Enabled)
                {
                    ImGui.SameLine();
                    ImGui.Checkbox("Override rail gun zoom", ref CameraFocus.OverrideRailGunZoom);

                    ImGui.InputFloat("FOV multiplier", ref CameraFocus.FOVMultiplier);
                    ImGui.InputFloat("Sensitivity multiplier", ref CameraFocus.SensitivityMultiplier);
                    ImGui.InputFloat("Zoom speed", ref CameraFocus.ZoomSpeed);
                   
                }

                ImGui.Checkbox("Custom FOV", ref CameraFocus.CustomFOVEnabled);

                if (CameraFocus.CustomFOVEnabled)
                {
                    ImGui.InputFloat("FOV", ref CameraFocus.CustomFOV);
                }
                ImGui.PopItemWidth();// 100 is too small for rgba colors
                // popping early because it's the last item, who cares
                ImGui.Checkbox("Custom crosshair color", ref CustomCrossHairColor.Enabled);

                if (CustomCrossHairColor.Enabled)
                {
                    Vector4 crosshaircolor = ColorHelper.ToVector4(CustomCrossHairColor.CrossHairColor);
                    if (ImGui.ColorEdit4("Crosshair color", ref crosshaircolor))
                    {
                        CustomCrossHairColor.SetColorFromVector4(crosshaircolor);
                    }
                }
            }
        }
    }
}
