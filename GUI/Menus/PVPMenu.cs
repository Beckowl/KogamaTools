using ImGuiNET;
using KogamaTools.Behaviours;
using KogamaTools.Helpers;
using KogamaTools.Tools.Misc;
using KogamaTools.Tools.PVP;
using UnityEngine;

namespace KogamaTools.GUI.Menus;

internal static class PVPMenu
{
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
        ImGui.SetNextItemWidth(-(GUIUtils.CalcButtonSize("Browse") + GUIUtils.CalcButtonSize("Clear") + ImGui.CalcTextSize("Custom crosshair") + GUIUtils.CalcSpacing(1) + ImGui.GetStyle().FramePadding).X);

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
                KogamaTools.mls.LogInfo(key.ToString());
                KeyRemapper.RemapControl<DesktopPlayMode>(kogamaControl, key);
            }
        }
    }
}
