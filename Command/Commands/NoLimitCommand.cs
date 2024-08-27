﻿using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.Command.Commands;

internal class NoLimitCommand : BaseCommand
{
    public NoLimitCommand() : base("/nolimit", "Disables the minumum cube count and modeling constraint of models/avatars.")
    {
        AddVariant(args => Toggle());
    }

    private void Toggle()
    {
        NoLimit.Enabled = !NoLimit.Enabled;
        NotificationHelper.NotifySuccess($"No limit {(NoLimit.Enabled ? "enabled" : "disabled")}.");
    }
}
