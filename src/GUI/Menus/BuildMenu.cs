using ImGuiNET;
using KogamaTools.Behaviours;
using KogamaTools.Tools.Build;

namespace KogamaTools.GUI.Menus;

internal static class BuildMenu
{
    internal static void Render()
    {
        if (!(MVGameControllerBase.GameMode == MV.Common.MVGameMode.Edit || MVGameControllerBase.GameMode == MV.Common.MVGameMode.CharacterEditor))
            return;

        if (!ImGui.BeginTabItem("Build"))
            return;

        ImGui.Checkbox("No build limit", ref NoLimit.Enabled);

        ImGui.Checkbox("Blue mode enabled", ref BlueModeToggle.BlueModeEnabled);

        ImGui.Checkbox("Single side painting", ref SingleSidePainting.Enabled);

        ImGui.Checkbox("Force destructibles selection", ref DestructiblesUnlock.Unlocked);

        ImGui.Checkbox("Avatar editor constraint enabled", ref EditModeSpeed.MovementConstrained);

        ImGui.Checkbox("Speed multiplier", ref EditModeSpeed.MultiplierEnabled);
        if (EditModeSpeed.MultiplierEnabled)
        {
            GUIUtils.InputFloat("Multiplier", ref EditModeSpeed.Multiplier);
        }

        if (MVGameControllerBase.GameMode == MV.Common.MVGameMode.Edit)
        {
            ImGui.Checkbox("Custom model scale", ref CustomModelScale.Enabled);

            if (CustomModelScale.Enabled)
            {
                GUIUtils.InputFloat("Scale", ref CustomModelScale.Scale);
            }

            ImGui.Checkbox("Custom rotation step", ref RotationStep.Enabled);

            if (RotationStep.Enabled)
            {
                GUIUtils.InputFloat("Rotation step", ref RotationStep.Step);
            }

            ImGui.Checkbox("Custom grid size", ref CustomGrid.Enabled);

            if (CustomGrid.Enabled)
            {
                GUIUtils.InputFloat("Grid size", ref CustomGrid.GridSize);
            }

            ImGui.Checkbox("Unlimited config", ref UnlimitedConfig.Enabled);

            if (UnlimitedConfig.Enabled)
            {
                GUIUtils.InputFloat("Minimum value", ref UnlimitedConfig.MinValue);
                GUIUtils.InputFloat("Maximum value", ref UnlimitedConfig.MaxValue);
            }

            ImGui.Checkbox("Multi select", ref MultiSelect.ForceSelection);
            if (MultiSelect.ForceSelection)
            {
                ImGui.SameLine();
                if (ImGui.Button("Group selected objects"))
                {
                    UnityMainThreadDispatcher.Instance.Enqueue(() => { ObjectGrouper.GroupSelectedObjects(); });
                }
            }

            ImGui.Checkbox("Fast links", ref FastLinks.Enabled);
            ImGui.Checkbox("Link fix", ref LinkFix.Enabled);
            ImGui.Checkbox("Force object links", ref ForceObjectLinks.Enabled);

            ImGui.Checkbox("Force interaction flags", ref ForceFlags.Enabled);
            if (ForceFlags.Enabled)
            {
                ImGui.SameLine();
                ImGui.Checkbox("Override", ref ForceFlags.Override);
                ShowInteractionFlags();
            }
        }

        ImGui.EndTabItem();
    }

    private static void ShowInteractionFlags()
    {
        InteractionFlags[] interactionFlags = (InteractionFlags[])Enum.GetValues(typeof(InteractionFlags));
        int flagCount = interactionFlags.Length;
        int midPoint = (flagCount + 1) / 2;

        if (ImGui.BeginTable("InteractionFlagsTable", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable))
        {
            for (int i = 0; i < midPoint; i++)
            {
                ImGui.TableNextColumn();

                RenderToggleFlag(interactionFlags[i]);

                ImGui.TableNextColumn();

                if (i + midPoint < flagCount)
                {
                    RenderToggleFlag(interactionFlags[i + midPoint]);
                }

                ImGui.TableNextRow();
            }

            ImGui.EndTable();
        }
    }

    private static void RenderToggleFlag(InteractionFlags flag)
    {
        bool flagSet = ForceFlags.AreFlagsSet(flag);
        if (ImGui.Checkbox(flag.ToString(), ref flagSet))
        {
            ForceFlags.ToggleFlags(flag);
        }
    }
}
