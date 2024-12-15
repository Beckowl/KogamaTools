﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Tools.PVP;

[HarmonyPatch]
internal class CameraFocus : MonoBehaviour
{
    internal static bool Enabled = true;
    internal static float FOVMultiplier = 0.6f;
    internal static float SensitivityMultiplier = 0.2f;
    internal static float ZoomSpeed = 5f;
    internal static bool OverrideRailGun = false;

    private static bool isFocusing = false;
    private static float zoomVelocity;

    private void Update()
    {
        if (!MVGameControllerBase.Game.IsPlaying || !Enabled) return;

        isFocusing = MVInputWrapper.GetBooleanControl(KogamaControls.PointerSelectAlt);

        DoZoom();
    }

    private void DoZoom()
    {
        float originalFOV = FOVModifier.CustomFOVEnabled ? FOVModifier.CustomFOV : 60f;
        float targetValue = isFocusing ? originalFOV * FOVMultiplier : originalFOV;

        MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView = Mathf.SmoothDamp(
            MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView,
            targetValue,
            ref zoomVelocity,
            1 / ZoomSpeed);
    }

    [HarmonyPatch(typeof(MVInputWrapper), "GetAxis")]
    [HarmonyPatch(typeof(MVInputWrapper), "GetAxisRaw")]
    [HarmonyPatch(typeof(MVInputWrapper), "GetAxisWithoutSensitivity")]
    [HarmonyPostfix]
    private static void GetAxis(ref float __result)
    {
        if (isFocusing)
        {
            __result *= SensitivityMultiplier;
        }
    }

    [HarmonyPatch(typeof(PickupItemRailGun), "DoChargingAnimation")]
    [HarmonyPrefix]
    private static bool DoChargingAnimation(PickupItemRailGun __instance)
    {
        if (!OverrideRailGun)
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
        if (!__instance.isCharging)
        {
            if (__instance.chargeParticles.isPlaying)
            {
                __instance.chargeParticles.Stop();
            }
            return false;
        }

        return true;
    }
}
