using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Features.PVP;

[HarmonyPatch]
internal static class CameraMod
{
    internal static bool CustomFOVEnabled = false;
    internal static float CustomFOV = 60;

    internal static CamFocusSettings FocusSettings = new CamFocusSettings();

    static CameraMod()
    {
        KogamaTools.Instance.AddComponent<FocusBehaviour>();
    }

    internal struct CamFocusSettings
    {
        internal bool CameraFocusEnabled = true;
        internal bool OverrideRailGunZoom = false;
        internal float SensitivityMultiplier = 0.2f;
        internal float FOVMultiplier = 0.6f;
        internal float FocusSpeed = 5f;

        public CamFocusSettings() { }
    }

    private class FocusBehaviour : MonoBehaviour
    {
        private float originalSensitivity;
        private float zoomVelocity = 0f;
        private bool isZooming = false;

        private void Update()
        {
            if (MVGameControllerBase.Game != null && MVGameControllerBase.Game.IsPlaying)
            {
                HandleFocus();
                DoZoom();
            }
        }

        private void HandleFocus()
        {
            if (!FocusSettings.CameraFocusEnabled)
            {
                return;
            }

            if (MVInputWrapper.GetBooleanControlDown(KogamaControls.PointerSelectAlt))
            {
                originalSensitivity = MVInputWrapper.MouseSensitivityModifier;
                MVInputWrapper.MouseSensitivityModifier *= FocusSettings.SensitivityMultiplier;
                isZooming = true;
            }
            else if (MVInputWrapper.GetBooleanControlUp(KogamaControls.PointerSelectAlt))
            {
                MVInputWrapper.MouseSensitivityModifier = originalSensitivity;
                isZooming = false;
            }
        }

        private void DoZoom()
        {
            float originalFOV = CustomFOVEnabled ? CustomFOV : 60f;
            float targetValue = isZooming ? originalFOV * FocusSettings.FOVMultiplier : originalFOV;

            MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView = Mathf.SmoothDamp(
                MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView,
                targetValue,
                ref zoomVelocity,
                1 / FocusSettings.FocusSpeed);
        }
    }

    // Rail gun FOV fix

    [HarmonyPatch(typeof(PickupItemRailGun), "DoChargingAnimation")]
    [HarmonyPrefix]
    private static bool DoChargingAnimation(PickupItemRailGun __instance)
    {
        if (!FocusSettings.OverrideRailGunZoom)
        {
            return true;
        }
        __instance.currentCharge = __instance.chargeCurve.Evaluate((Time.time - __instance.chargeBeginTime) / __instance.curveChargeLength);
        __instance.chargeAudioSource.pitch = 0.2f + __instance.currentCharge;
        __instance.chargeParticles.time = __instance.currentCharge;

        return false;
    }

    [HarmonyPatch(typeof(PickupItemRailGun), "Update")]
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
