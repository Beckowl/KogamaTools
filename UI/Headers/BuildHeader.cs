using ImGuiNET;
using KogamaTools.Patches;

namespace KogamaTools.UI.Headers
{
    internal static class BuildHeader
    {
        // this is just a big test, do not judge me from this code lol
        // i'm prob going to make a GUI wrapper

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
                ImGui.PopItemWidth();
            }
        }
    }
}
