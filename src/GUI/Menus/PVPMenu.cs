using ImGuiNET;
using KogamaTools.Behaviours;
using KogamaTools.Helpers;
using KogamaTools.Tools.Graphics;
using KogamaTools.Tools.Misc;
using KogamaTools.Tools.PVP;
using UnityEngine;

namespace KogamaTools.GUI.Menus;

internal static class PVPMenu
{
    internal static void Render()
    {

        if (MVGameControllerBase.GameMode == MV.Common.MVGameMode.CharacterEditor)
            return;

        if (!ImGui.BeginTabItem("PvP"))
            return;

        ImGui.Checkbox("Fast respawn", ref FastRespawn.Enabled);

        if (FastRespawn.Enabled)
        {
            ImGui.SameLine();
            ImGui.Checkbox("Respawn at safe spot", ref FastRespawn.RespawnAtSafeSpot);
        }

        ImGui.Checkbox("Anti AFK", ref AntiAFK.Enabled);

        ImGui.Checkbox("Force third person in gun mode", ref ForceThirdPersonCamera.Enabled);

        ImGui.Checkbox("Camera Focus", ref CameraFocus.Enabled);

        if (CameraFocus.Enabled)
        {
            ImGui.SameLine();
            ImGui.Checkbox("Override rail gun zoom", ref CameraFocus.OverrideRailGun);

            GUIUtils.InputFloat("FOV multiplier", ref CameraFocus.FOVMultiplier);
            GUIUtils.InputFloat("Sensitivity multiplier", ref CameraFocus.SensitivityMultiplier);
            GUIUtils.InputFloat("Zoom speed", ref CameraFocus.ZoomSpeed);
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

        ImGui.SetNextItemWidth(-GUIUtils.CalcReservedButtonSpaceLabel("Custom crosshair", "Browse", "Load"));

        ImGui.InputText("Custom crosshair", ref CustomCrossHairTexture.TexturePath, 260);

        ImGui.SameLine();

        if (ImGui.Button("Browse"))
        {
            FileDialog.OpenFile("psd,tiff,jpg,tga,png,gif,bmp,iff,pict", null, (result) =>
            {
                if (result.IsOk)
                {
                    CustomCrossHairTexture.TexturePath = result.Path;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => CustomCrossHairTexture.SetTexture());
                }
            });
        }

        ImGui.SameLine();

        if (ImGui.Button("Load"))
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => CustomCrossHairTexture.SetTexture());
        }

        if (GUIUtils.InputFloat("Camera distance to avatar", ref CameraDistanceModifier.distance))
        {
            CameraDistanceModifier.ApplyChanges();
        }

        ImGui.Separator();

        ImGui.Text("Keybinds");

        if (!ImGui.BeginTable("Table", 2, ImGuiTableFlags.Borders)) return;

        ImGui.TableSetupColumn("Control");
        ImGui.TableSetupColumn("Key");
        ImGui.TableHeadersRow();

        RenderControls();

        ImGui.EndTable();
        ImGui.EndTabItem();
    }

    private static void RenderControls()
    {
        foreach (PlayControls control in Enum.GetValues(typeof(PlayControls)))
        {
            string controlStr = control.ToString();
            KogamaControls kogamaControl = (KogamaControls)Enum.Parse(typeof(KogamaControls), controlStr);
            KeyCode key = KeyRemapper.GetKeyCodeForControl<DesktopPlayMode>(kogamaControl);

            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            ImGui.Text(controlStr);
            ImGui.TableSetColumnIndex(1);

            if (GUIUtils.RenderEnum("##" + controlStr, ref key))
            {
                KeyRemapper.RemapControl<DesktopPlayMode>(kogamaControl, key);
            }

            ImGui.SameLine();
            if (ImGui.SmallButton("Reset##" + controlStr))
            {
                KeyRemapper.ResetToDefaults<DesktopPlayMode>(kogamaControl);
            }
        }
    }
}
