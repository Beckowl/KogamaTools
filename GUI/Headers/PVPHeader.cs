using System.Numerics;
using System.Text;
using ImGuiNET;
using KogamaTools.Behaviours;
using KogamaTools.Helpers;
using KogamaTools.Patches;
using UnityEngine;

using UnityEngine.UI;

namespace KogamaTools.GUI.Headers
{
    internal static class PVPHeader
    {

        private static byte[] customCrosshairPath = new byte[1024];

        private static void SetCrosshairTexture(string filePath) // i should move this to somewhere else
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                Texture2D tex = TextureHelper.LoadPNG(filePath);

                if (tex == null)
                {
                    NotificationHelper.NotifyError($"Invalid file path {filePath}.");
                    return;
                }

                CrossHair crosshair = MVGameControllerBase.PlayModeUI.GetCrossHair().Cast<CrossHair>();

                Image image = crosshair.crossHair;
                Sprite sprite = image.sprite;
                UnityEngine.Vector2 pivot = sprite.pivot;
                Rect rect = sprite.rect;

                Sprite newsprite = Sprite.Create(tex, rect, pivot);
                image.sprite = newsprite;
            });
        }

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

                ImGui.PopItemWidth();

                ImGui.Checkbox("Custom crosshair color", ref CustomCrossHairColor.Enabled);

                if (CustomCrossHairColor.Enabled)
                {
                    System.Numerics.Vector4 crosshaircolor = ColorHelper.ToVector4(CustomCrossHairColor.CrossHairColor);
                    if (ImGui.ColorEdit4("Crosshair color", ref crosshaircolor))
                    {
                        CustomCrossHairColor.SetColorFromVector4(crosshaircolor);
                    }
                }

                ImGui.PushItemWidth(180);

                ImGui.InputText("Custom crosshair", customCrosshairPath, (uint)customCrosshairPath.Length);

                ImGui.SameLine();

                if (ImGui.Button("Load"))
                {
                    string path = Encoding.UTF8.GetString(customCrosshairPath).TrimEnd('\0');

                    SetCrosshairTexture(path);
                }

                ImGui.SameLine();

                if (ImGui.Button("Clear"))
                {
                    customCrosshairPath = new byte[1024];
                }

                ImGui.PopItemWidth();
            }
        }
    }
}
