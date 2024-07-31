using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(MV.WorldObject.MVMath))]
    internal class FloatErrorFix
    {
        [HarmonyPatch("TryValidateFloat")]
        [HarmonyPrefix]
        private static bool TryValidateFloat(ref float f, ref float __result)
        {
            KogamaTools.mls.LogInfo(f);
            if (f == float.NaN)
            {
                __result = 0;
                return false;
            }
            return true;
        }
    }
}
