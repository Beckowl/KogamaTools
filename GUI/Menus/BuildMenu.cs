using ImGuiNET;
using KogamaTools.Behaviours;
using KogamaTools.Tools.Build;

namespace KogamaTools.GUI.Menus;

internal static class BuildMenu
{
    private static void ShowInteractionFlags()
    {
        InteractionFlags[] interactionFlags = (InteractionFlags[])Enum.GetValues(typeof(InteractionFlags));

        ImGui.Separator();
        ImGui.Columns(2);
        for (int i = 0; i < interactionFlags.Length; i++)
        {
            bool flagSet = ForceFlags.AreFlagsSet(interactionFlags[i]); // dummy

            if (ImGui.Checkbox(interactionFlags[i].ToString(), ref flagSet))
            {
                ForceFlags.ToggleFlags(interactionFlags[i]);
            }
            if (i == interactionFlags.Length / 2)
            {
                ImGui.NextColumn();
            }
        }
        ImGui.Columns(1);
        ImGui.Separator();
    }

    internal static void Render()
    {
        if (!ImGui.BeginTabItem("Build"))
            return;

        ImGui.PushItemWidth(100);
        ImGui.Checkbox("No build limit", ref NoLimit.Enabled);

        ImGui.Checkbox("Blue mode enabled", ref BlueModeController.BlueModeEnabled);
        ImGui.Checkbox("Avatar editor constraint enabled", ref EditModeMovement.MovementConstraintEnabled);

        ImGui.Checkbox("Speed multiplier", ref EditModeMovement.SpeedMultEnabled);

        ImGui.Checkbox("Single side painting", ref SingleSidePainting.Enabled);
        ImGui.Checkbox("Custom model scale", ref CustomModelScale.Enabled);

        if (CustomModelScale.Enabled)
        {
            ImGui.InputFloat("Scale", ref CustomModelScale.CustomScale);
        }

        ImGui.Checkbox("Custom rotation step", ref RotationStep.Enabled);

        if (RotationStep.Enabled)
        {
            ImGui.InputFloat("Rotation step", ref RotationStep.Step);
        }

        ImGui.Checkbox("Custom grid size", ref CustomGrid.Enabled);

        if (CustomGrid.Enabled)
        {
            ImGui.InputFloat("Grid size", ref CustomGrid.GridSize);
        }

        ImGui.Checkbox("Unlimited config", ref UnlimitedConfig.Enabled);

        if (UnlimitedConfig.Enabled)
        {
            ImGui.InputFloat("Minimum value", ref UnlimitedConfig.MinValue);
            ImGui.InputFloat("Maximum value", ref UnlimitedConfig.MaxValue);
        }

        ImGui.Checkbox("Multi select", ref MultiSelect.ForceSelection);
        if (MultiSelect.ForceSelection)
        {
            ImGui.SameLine();
            if (ImGui.Button("Group selected objects"))
            {
                ObjectGrouper.GroupSelectedObjects();
            }
        }
        ImGui.Checkbox("Group edit (BETA)", ref GroupEdit.Enabled);

        if (EditModeMovement.SpeedMultEnabled)
        {
            ImGui.InputFloat("Multiplier", ref EditModeMovement.SpeedMult);
        }

        ImGui.Checkbox("Fast links", ref FastLinks.Enabled);
        ImGui.Checkbox("Link fix", ref LinkFix.Enabled);

        ImGui.Checkbox("Force interaction flags", ref ForceFlags.Enabled);
        if (ForceFlags.Enabled)
        {
            ImGui.SameLine();
            ImGui.Checkbox("Override", ref ForceFlags.Override);
            ShowInteractionFlags();
        }

        ImGui.PopItemWidth();
        ImGui.EndTabItem();
    }
}
