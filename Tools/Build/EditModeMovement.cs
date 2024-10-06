using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class EditModeMovement
{
    internal static float SpeedMult = 2;
    internal static bool SpeedMultEnabled = false;
    internal static bool MovementConstraintEnabled = true;

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
