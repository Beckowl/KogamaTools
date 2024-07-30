using System;
using HarmonyLib;
using MV.Common;

namespace KogamaTools.Patches
{
    internal static class NoCheatHandling
    {
        [HarmonyPatch(typeof(MVNetworkGame.OperationRequests))]
        private static class OperationRequestsPatches
        // most of these aren't needed, but there's no harm in keeping them nice and ineffective, right?
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

        [HarmonyPatch(typeof(CheatHandling))]
        private static class CheatHandlingPatches
        {
            [HarmonyPatch("Init")]
            [HarmonyPrefix]
            static bool Init()
            {
                return false;
            }

            [HarmonyPatch("MachineBanDetected")]
            [HarmonyPrefix]
            static bool MachineBanDetected()
            {
                return false;
            }

            [HarmonyPatch("ExecuteBan")]
            [HarmonyPrefix]
            static bool ExecuteBan()
            {
                return false;
            }
        }
    }
}

