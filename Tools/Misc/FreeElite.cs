using HarmonyLib;

namespace KogamaTools.Tools.Misc;
internal class FreeElite
{
    [HarmonyPatch(typeof(MVClientSettings), nameof(MVClientSettings.IsSubscriber), MethodType.Getter)]
    [HarmonyPostfix]
    private static void EliteGetter(ref bool __result)
    {
        __result = true;
    }

    [HarmonyPatch(typeof(MVClientSettings), nameof(MVClientSettings.IsSubscriber), MethodType.Setter)]
    [HarmonyPrefix]
    private static bool EliteSetter(ref bool value)
    {
        value = true;
        return false;
    }
}
