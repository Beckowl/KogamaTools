using HarmonyLib;
using MV.Common;

namespace KogamaTools.Tools.PVP;

[HarmonyPatch]
internal class FastRespawn
{
    internal static bool Enabled = false;
    internal static bool RespawnAtSafeSpot = false;
    private static void RespawnPlayer()
    {
        MVGameControllerBase.GameEventManager.AvatarCommandsPlayMode.SetToSpawnPoint();

        if (RespawnAtSafeSpot)
        {
            SetToSafeSpot();
        }

        MVGameControllerBase.GameEventManager.AvatarCommandsPlayMode.EnterPlayingState();
    }

    private static void SetToSafeSpot()
    {
        MVAvatarLocal avatarLocal = MVGameControllerBase.Game.LocalPlayer.AvatarLocal;

        if (avatarLocal.interactableLocal.LastDamageSource == null || avatarLocal.interactableLocal.LastDamageSource.Outdated)
        {
            ReviveState reviveState = MVGameControllerBase.SpawnRoleDataMediatorLocal.reviveState;
            if (reviveState.CanSafelySpawn)
            {
                MVGameControllerBase.GameEventManager.AvatarCommandsPlayMode.MoveBodyToSafeSpot(reviveState.safePositions.Count - 1);
            }
        }
    }

    [HarmonyPatch(typeof(MVAvatarLocal), "Suicide")]
    [HarmonyPrefix]
    private static bool Suicide(MVAvatarLocal __instance)
    {
        if (!Enabled) return true;

        if (!__instance.IsInMode(SpawnRoleModeType.Dead))
        {
            if (__instance.interactableLocal.LastDamageSource == null || __instance.interactableLocal.LastDamageSource.Outdated)
            {
                __instance.Die();
                RespawnPlayer();
            }
            else
            {
                __instance.interactableLocal.DieFromRespawn(__instance.interactableLocal.LastDamageSource.shooter, __instance.interactableLocal.LastDamageSource.damageType);
            }
        }

        return false;
    }

    [HarmonyPatch(typeof(AvatarInteractable), "DoKilledNotification")]
    [HarmonyPatch(typeof(AvatarInteractable), "DieFromFalling")]
    [HarmonyPatch(typeof(AvatarInteractable), "DieFromStuck")]
    [HarmonyPatch(typeof(AvatarInteractable), "DieFromBeingStuck")]
    [HarmonyPrefix]
    private static void DoKilledNotification(AvatarInteractable __instance)
    {
        if (Enabled)
        {
            RespawnPlayer();
        }
    }

    [HarmonyPatch(typeof(DeathUIController), "StartDeathBriefing")]
    [HarmonyPrefix]
    private static bool StartDeathBriefing()
    {
        return !Enabled;
    }
}
