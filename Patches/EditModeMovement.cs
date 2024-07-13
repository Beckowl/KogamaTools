using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(MVBuildModeAvatarLocal.EditMode))]
    internal static class EditModeMovement
    {
        public static float speedMult = 1f;
        public static bool speedMultEnabled = false;
        public static bool movementConstraintEnabled = false;

        [HarmonyPatch("MoveCharacter")]
        [HarmonyPrefix]
        static void MoveCharacter(ref Vector3 moveDelta, MVBuildModeAvatarLocal.EditMode __instance)
        {
            if (speedMultEnabled)
            {
                moveDelta.x *= speedMult;
                moveDelta.y *= speedMult;
                moveDelta.z *= speedMult;
            }

            __instance.MovementConstrained = __instance.MovementConstrained && movementConstraintEnabled;
        }
    }
}
