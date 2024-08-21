using HarmonyLib;
using KogamaTools.Behaviours;
using UnityEngine;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(PickupItemRailGun))]
    internal static class RailGunFOV
    {
        [HarmonyPatch("DoChargingAnimation")]
        [HarmonyPrefix]
        private static bool DoChargingAnimation(PickupItemRailGun __instance)
        {
            if (!CameraFocus.OverrideRailGunZoom)
            {
                return true;
            }
            __instance.currentCharge = __instance.chargeCurve.Evaluate((Time.time - __instance.chargeBeginTime) / __instance.curveChargeLength);
            __instance.chargeAudioSource.pitch = 0.2f + __instance.currentCharge;
            __instance.chargeParticles.time = __instance.currentCharge;

            return false;
        }

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static bool Update(PickupItemRailGun __instance)
        {
            if (__instance.isCharging)
            {
                __instance.DoChargingAnimation();
                if (!__instance.chargeAudioSource.isPlaying)
                {
                    __instance.chargeAudioSource.Play();
                }
                if (!__instance.chargeParticles.isPlaying)
                {
                    __instance.chargeParticles.Play();
                }
            }
            else
            {
                if (__instance.chargeParticles.isPlaying)
                {
                    __instance.chargeParticles.Stop();
                }
            }
            return false;
        }
    }
}
