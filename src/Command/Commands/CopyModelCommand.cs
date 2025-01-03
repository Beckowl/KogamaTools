﻿using KogamaTools.Helpers;
using KogamaTools.Tools.Build;
using static KogamaTools.Helpers.ModelHelper;

namespace KogamaTools.Command.Commands;
[CommandName("/copymodel")]
[CommandDescription("Copies data from the target model so it can be pasted into other models.")]
[BuildModeOnly]
internal class CopyModelCommand : BaseCommand
{
    [CommandVariant]
    private void CopyModel()
    {
        MVCubeModelBase targetModel = GetTargetModel();

        if (targetModel == null)
        {
            NotificationHelper.WarnUser("Could not find target cube model.");
            return;
        }

        if (!CanExportModel(targetModel))
        {
            NotificationHelper.WarnUser("You do not own this model.");
            return;
        }

        CopyPasteModel.CopyModel(targetModel);
    }

    private bool CanExportModel(MVCubeModelBase model)
    {
        if (model.id == 75579)
        {
            if (MVGameControllerBase.Game.LocalPlayer.PlanetOwnership != MVLocalPlayer.PlanetOwnershipType.Owner)
            {
                return false;
            }
            return true;
        }

        if (!IsModelOwner(model))
        {
            return false;
        }

        return true;
    }
}
