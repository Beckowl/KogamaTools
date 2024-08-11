using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Patches
{
    internal static class FastRespawn
    {
        internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("FastRespawnEnabled");

        private static bool RespawnPlayer()
        {
            if (Enabled && (MVGameControllerBase.Game.IsPlaying))
            {
                MVGameControllerBase.GameEventManager.AvatarCommandsPlayMode.SetToSpawnPoint();
                MVGameControllerBase.GameEventManager.AvatarCommandsPlayMode.EnterPlayingState();
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(DesktopPlayModeController), "Respawn")]
        [HarmonyPrefix]
        static bool Respawn()
        {
            return RespawnPlayer();
        }

        [HarmonyPatch(typeof(DeathUIController), "OnLocalPlayerKilled")]
        [HarmonyPrefix]
        static bool OnLocalPlayerKilled()
        {
            return RespawnPlayer();
        }
    }
}
