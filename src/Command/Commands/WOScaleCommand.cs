using UnityEngine;
using KogamaTools.Tools.Build;
using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands;

[CommandName("/woscale")]
[BuildModeOnly]
[CommandDescription("Defines a custom scale for new world objects.")]
internal class WOScaleCommand : BaseCommand
{
    [CommandVariant]
    private void SetScale(float x, float y, float z)
    {
        Vector3 scale = new Vector3(x, y, z);

        CustomWOScale.Enabled = true;
        CustomWOScale.Scale = scale;
        CustomWOScale.RequestNewGroupIfNecessary();

        NotificationHelper.NotifySuccess($"World object scale set to {scale.ToString()}.");
    }

    [CommandVariant]
    private void Toggle()
    {
        CustomWOScale.Enabled = !CustomWOScale.Enabled;

        NotificationHelper.NotifySuccess($"Custom world object scale {(CustomWOScale.Enabled ? "enabled" : "disabled")}.");
    }
}
