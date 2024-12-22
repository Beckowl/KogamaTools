using HarmonyLib;

namespace KogamaTools.Tools.Misc;
[HarmonyPatch]
internal static class FreeElite
{
    [HarmonyPatch(typeof(MVClientSettings), nameof(MVClientSettings.IsSubscriber), MethodType.Getter)]
    [HarmonyPostfix]
    private static void EliteGetter(ref bool __result)
    {
        __result = true;
    }
}
