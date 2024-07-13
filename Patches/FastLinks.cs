using System;
using HarmonyLib;
using MV.WorldObject;
using static LogicObjectManager;

namespace KogamaTools.patches
{
    [HarmonyPatch(typeof(LogicObjectManager))]
    internal static class FastLinks
    {
        public static bool Enabled = false;

        [HarmonyPatch("ValidateLink", new Type[] { typeof(int), typeof(int), typeof(IWorldObjectManager), typeof(bool) },
            new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Ref })]
        [HarmonyPrefix]
        public static void ValidateLink(ref bool loopDetected)
        {
            if (Enabled)
            {
                loopDetected = false;
            }
        }

        [HarmonyPatch("ValidateLink", new Type[] { typeof(int), typeof(int), typeof(IWorldObjectManager), typeof(bool), typeof(LogicObjectManager.ReportSeverity) },
            new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out })]
        [HarmonyPrefix]
        public static bool ValidateLink(ref ValidateLinkStatus __result, ref LogicObjectManager.ReportSeverity reportSeverity)
        {
            if (Enabled)
            {
                __result = ValidateLinkStatus.Ok;
                reportSeverity = ReportSeverity.Info;
                return false;
            }
            return true;
        }
    }
}