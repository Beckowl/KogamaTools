using HarmonyLib;
using KogamaTools.Helpers;
using MV.WorldObject;
using static LogicObjectManager;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class FastLinks
{
    internal static bool Enabled = false;

    [HarmonyPatch(typeof(LogicObjectManager), "ValidateLink", new Type[] { typeof(int), typeof(int), typeof(IWorldObjectManager), typeof(bool) },
                new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Ref })]
    [HarmonyPrefix]
    public static bool ValidateLink(ref bool loopDetected)
    {
        if (Enabled)
        {
            loopDetected = false;
            return false;
        }
        return true;
    }

    [HarmonyPatch(typeof(LogicObjectManager), "ValidateLink", new Type[] { typeof(int), typeof(int), typeof(IWorldObjectManager), typeof(bool), typeof(ReportSeverity) },
        new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out })]
    [HarmonyPrefix]
    public static bool ValidateLink(ref ValidateLinkStatus __result, ref ReportSeverity reportSeverity)
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