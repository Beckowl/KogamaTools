using HarmonyLib;
using KogamaTools.Config;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
[Section("Build")]
internal static class EditModeSpeed
{
    [Bind] internal static bool MultiplierEnabled = false;
    [Bind] internal static float Multiplier = 2;
    [Bind] internal static bool MovementConstrained = true;

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

        __instance.MovementConstrained = __instance.MovementConstrained && MovementConstrained;
    }
}
