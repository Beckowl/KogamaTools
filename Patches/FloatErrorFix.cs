using HarmonyLib;

namespace KogamaTools.Patches
{
    internal static class FloatErrorFix
    {
        [HarmonyPatch(typeof(MV.WorldObject.MVMath), "TryValidateFloat")]
        [HarmonyPrefix]
        private static bool TryValidateFloat(ref float f, ref float __result)
        {
            if (float.IsNaN(f))
            {
                __result = 0f;
                return false;
            }
            return true;
        }
    }
}

