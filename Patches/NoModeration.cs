using System;
using HarmonyLib;
using MV.Common;

namespace KogamaTools.patches
{
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests))]
    internal class NoModeration
    {
        [HarmonyPatch("RevokeEditRights")]
        [HarmonyPrefix]
        static bool RevokeEditRights()
        {
            return false;
        }

        [HarmonyPatch("Ban", new Type[] { typeof(int), typeof(MVPlayer), typeof(string) })]
        [HarmonyPrefix]
        static bool Ban(ref int hours, ref MVPlayer target, ref string reason)
        {
            return false;
        }

        [HarmonyPatch("Ban", typeof(CheatType))]
        [HarmonyPrefix]
        static bool Ban(ref CheatType cheatType)
        {
            return false;
        }

        [HarmonyPatch("Expel")]
        [HarmonyPrefix]
        static bool Expel()
        {
            return false;
        }

        [HarmonyPatch("Kick")]
        [HarmonyPrefix]
        static bool Kick()
        {
            return false;
        }
    }
}

