using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands;

[CommandName("/teleport")]
[CommandName("/tp")] // alias
[CommandDescription("Teleports you to a specified player's position.")]
[BuildModeOnly]
internal class TeleportCommand : BaseCommand
{
    [CommandVariant]
    private void Teleport(string playerName)
    {
        MVPlayer target = AdminToolController.GetPlayer(playerName);
        if (target == null)
        {
            NotificationHelper.WarnUser($"Target player \"{playerName}\" not found. Check your capitalization and ensure it matches exactly.");
            return;
        }

        MVWorldObjectClient WO = MVGameControllerBase.WOCM.GetWorldObjectClient(target.WoId);
        if (WO == null)
        {
            NotificationHelper.NotifyError("Target player WOCM not found.");
            return;
        }

        MVGameControllerBase.MainCameraManager.CurrentCamera.FocusOnObject(WO);
        NotificationHelper.NotifySuccess($"Teleported you to {playerName}.");
    }
}
