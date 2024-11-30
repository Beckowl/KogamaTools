using HarmonyLib;
using MV.Common;

namespace KogamaTools.Tools.PVP
{
    [HarmonyPatch]
    internal class FastRespawn
    {
        internal static bool Enabled = false;
        private static void RespawnPlayer()
        {
            MVGameControllerBase.GameEventManager.AvatarCommandsPlayMode.SetToSpawnPoint();
            MVGameControllerBase.GameEventManager.AvatarCommandsPlayMode.EnterPlayingState();
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
        private static bool OnSetToDeadMode()
        {
            return !Enabled;
        }
    }
}
