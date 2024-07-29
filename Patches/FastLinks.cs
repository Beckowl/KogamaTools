using System;
using HarmonyLib;
using KogamaTools.Helpers;
using MV.WorldObject;
using static LogicObjectManager;

namespace KogamaTools.patches
{
    [HarmonyPatch(typeof(LogicObjectManager))]
    internal static class FastLinks
    {
        internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("FastLinksEnabled");

        [HarmonyPatch("ValidateLink", [typeof(int), typeof(int), typeof(IWorldObjectManager), typeof(bool)],
            [ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Ref])]
        [HarmonyPrefix]
        private static void ValidateLink(ref bool loopDetected)
        {
            if (Enabled)
            {
                loopDetected = false;
            }
        }

        [HarmonyPatch("ValidateLink", [typeof(int), typeof(int), typeof(IWorldObjectManager), typeof(bool), typeof(LogicObjectManager.ReportSeverity)],
            [ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out])]
        [HarmonyPrefix]
        private static bool ValidateLink(ref ValidateLinkStatus __result, ref LogicObjectManager.ReportSeverity reportSeverity)
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