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
                TextCommand.NotifyUser($"<color=yellow>Target player \"{player}\" not found. Check your capitalization and ensure it matches exactly.</color>");
                return;
            }

            MVWorldObjectClient WO = MVGameControllerBase.WOCM.GetWorldObjectClient(target.WoId);

            if (WO == null)
            {
                TextCommand.NotifyUser("<color=yellow>Target player WOCM not found.</color>");
                return;
            }

            MVGameControllerBase.MainCameraManager.CurrentCamera.FocusOnObject(WO);
            TextCommand.NotifyUser($"<color=cyan>Teleported to {player}.</color>");
        }
    }
}
