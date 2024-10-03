using HarmonyLib;

namespace KogamaTools.Tools.Build;
internal class FreeElite
{
    [HarmonyPatch(typeof(MVClientSettings), nameof(MVClientSettings.IsSubscriber), MethodType.Getter)]
    [HarmonyPostfix]
    private static void elite(ref bool __result)
    {
        __result = true;
    }
}
