using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Il2CppSystem.Resources;
using ImGuiNET;
using KogamaTools.Behaviours;
using KogamaTools.Patches;

namespace KogamaTools.UI.Headers
{
    internal static class PVPHeader
    {
        private static Vector3 color = new Vector3();
        internal static void Render()
        {
            if (ImGui.CollapsingHeader("PvP"))
            {
                ImGui.PushItemWidth(100);
                ImGui.Checkbox("Fast respawn", ref FastRespawn.Enabled);
                ImGui.Checkbox("Camera Focus", ref CameraFocus.instance.enabled);
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
