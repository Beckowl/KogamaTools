using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Features.PVP;

internal static class FastRespawn
{
    internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("FastRespawnEnabled");

    private static bool RespawnPlayer()
    {
        if (Enabled && MVGameControllerBase.Game.IsPlaying)
        {
            MVGameControllerBase.GameEventManager.AvatarCommandsPlayMode.SetToSpawnPoint();
            MVGameControllerBase.GameEventManager.AvatarCommandsPlayMode.EnterPlayingState();
            return false;
        }
        return true;
    }

    [HarmonyPatch(typeof(DesktopPlayModeController))]
    private static class DesktopPlayModeControllerPatch
    {

        [HarmonyPatch("Respawn")]
        [HarmonyPrefix]
        static bool Respawn()
        {
            return RespawnPlayer();
        }
    }

    [HarmonyPatch(typeof(DeathUIController))]
    private static class DeathUIControllerPatch
    {
        [HarmonyPatch("OnLocalPlayerKilled")]
        [HarmonyPrefix]
        static bool OnLocalPlayerKilled()
        {
            return RespawnPlayer();
        }
    }
}
