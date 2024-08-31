using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Features.Build;

[HarmonyPatch]
internal static class EditModeMovement
{
    internal static float SpeedMult = ConfigHelper.GetConfigValue<float>("SpeedMult");
    internal static bool SpeedMultEnabled = ConfigHelper.GetConfigValue<bool>("SpeedMultEnabled");
    internal static bool MovementConstraintEnabled = ConfigHelper.GetConfigValue<bool>("MovementConstraintEnabled");

    [HarmonyPatch(typeof(MVBuildModeAvatarLocal.EditMode), "MoveCharacter")]
    [HarmonyPrefix]
    private static void MoveCharacter(ref Vector3 moveDelta, MVBuildModeAvatarLocal.EditMode __instance)
    {
        if (SpeedMultEnabled)
        {
            moveDelta.x *= SpeedMult;
            moveDelta.y *= SpeedMult;
            moveDelta.z *= SpeedMult;
        }

        __instance.MovementConstrained = __instance.MovementConstrained && MovementConstraintEnabled;
    }
}
