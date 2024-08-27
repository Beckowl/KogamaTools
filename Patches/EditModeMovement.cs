using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Patches;

[HarmonyPatch(typeof(MVBuildModeAvatarLocal.EditMode))]
internal static class EditModeMovement
{
    internal static float SpeedMult = ConfigHelper.GetConfigValue<Single>("SpeedMult");
    internal static bool SpeedMultEnabled = ConfigHelper.GetConfigValue<bool>("SpeedMultEnabled");
    internal static bool MovementConstraintEnabled = ConfigHelper.GetConfigValue<bool>("MovementConstraintEnabled");

    [HarmonyPatch("MoveCharacter")]
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
