﻿using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class EditModeSpeed
{
    internal static float Multiplier = 2;
    internal static bool MultiplierEnabled = false;
    internal static bool MovementConstraintEnabled = true;

    [HarmonyPatch(typeof(MVBuildModeAvatarLocal.EditMode), "MoveCharacter")]
    [HarmonyPrefix]
    private static void MoveCharacter(ref Vector3 moveDelta, MVBuildModeAvatarLocal.EditMode __instance)
    {
        if (MultiplierEnabled)
        {
            moveDelta.x *= Multiplier;
            moveDelta.y *= Multiplier;
            moveDelta.z *= Multiplier;
        }

        __instance.MovementConstrained = __instance.MovementConstrained && MovementConstraintEnabled;
    }
}