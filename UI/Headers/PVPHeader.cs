using System.Numerics;
using ImGuiNET;
using KogamaTools.Behaviours;
using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.UI.Headers
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
                    ImGui.InputFloat("FOV multiplier", ref CameraFocus.FOVMultiplier);
                    ImGui.InputFloat("Sensitivity multiplier", ref CameraFocus.SensitivityMultiplier);
                    ImGui.InputFloat("Zoom speed", ref CameraFocus.ZoomSpeed);
                }

                ImGui.Checkbox("Custom FOV", ref CameraPatch.CustomFOVEnabled);

                if (CameraPatch.CustomFOVEnabled)
                {
                    ImGui.InputFloat("FOV", ref CameraPatch.CustomFOV);
                }

                ImGui.Checkbox("Custom crosshair color", ref CustomCrossHairColor.Enabled);

                if (CustomCrossHairColor.Enabled)
                {
                    ImGui.ColorEdit3("Crosshair color", ref CustomCrossHairColor.CrossHairColor);
                }

                ImGui.PopItemWidth();
            }
        }
    }
}
