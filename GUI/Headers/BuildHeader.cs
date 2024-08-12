using ImGuiNET;
using KogamaTools.Patches;

namespace KogamaTools.GUI.Headers
{
    internal static class BuildHeader
    {
        // this is just a big test, do not judge me from this code lol
        // i'm prob going to make a GUI wrapper
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

        // long ass method
        internal static void Render()
        {
            if (ImGui.CollapsingHeader("Build"))
            {
                ImGui.PushItemWidth(100); // set inputFloat size
                ImGui.Checkbox("No build limit", ref NoLimit.Enabled);
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

                ImGui.Checkbox("Unlimited config", ref UnlimitedConfig.Enabled);

                if (UnlimitedConfig.Enabled)
                {
                    ImGui.InputFloat("Minimum value", ref UnlimitedConfig.MinValue);
                    ImGui.InputFloat("Maximum value", ref UnlimitedConfig.MaxValue);
                }

                ImGui.Checkbox("Custom grid size", ref CustomGrid.Enabled);

                if (CustomGrid.Enabled)
                {
                    ImGui.InputFloat("Grid size", ref CustomGrid.GridSize);
                }

                ImGui.Checkbox("Multi select", ref MultiSelect.ForceSelection);
                ImGui.Checkbox("Movement constraint enabled", ref EditModeMovement.MovementConstraintEnabled);
                ImGui.Checkbox("Speed multiplier", ref EditModeMovement.SpeedMultEnabled);

                if (EditModeMovement.SpeedMultEnabled)
                {
                    ImGui.InputFloat("Multiplier", ref EditModeMovement.SpeedMult);
                }

                ImGui.Checkbox("Blue mode enabled", ref CameraPatch.BlueModeEnabled);
                ImGui.Checkbox("Fast links", ref FastLinks.Enabled);
                ImGui.Checkbox("Force interaction flags", ref ForceFlags.Enabled);
                if (ForceFlags.Enabled)
                {
                    ImGui.SameLine();
                    ImGui.Checkbox("Override", ref ForceFlags.Override);
                    ShowInteractionFlags();
                }
                ImGui.PopItemWidth();
            }
        }
    }
}
