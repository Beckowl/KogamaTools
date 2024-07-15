using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands
{
    internal class TeleportCommand : BaseCommand
    {
        public TeleportCommand() : base("/teleport", "Teleports the camera view to a specified player's position.")
        {
            AddVariant(args => Teleport((string)args[0]), typeof(string));
        }

        private void Teleport(string player)
        {

            MVPlayer target = AdminToolController.GetPlayer(player);
            if (target == null)
            {
                NotificationHelper.WarnUser($"Target player \"{player}\" not found. Check your capitalization and ensure it matches exactly.");
                return;
            }

            MVWorldObjectClient WO = MVGameControllerBase.WOCM.GetWorldObjectClient(target.WoId);
            if (WO == null)
            {
                NotificationHelper.WarnUser("Target player WOCM not found.");
                return;
            }

            MVGameControllerBase.MainCameraManager.CurrentCamera.FocusOnObject(WO);
            NotificationHelper.NotifySuccess($"Teleported you to {player}.");
        }
    }
}
