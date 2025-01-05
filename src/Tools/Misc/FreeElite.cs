using HarmonyLib;
using KogamaTools.Config;

namespace KogamaTools.Tools.Misc;
[HarmonyPatch]
[Section("Misc")]
internal static class FreeElite
{
    [Bind] internal static bool Enabled = true;

    [HarmonyPatch(typeof(MVClientSettings), nameof(MVClientSettings.IsSubscriber), MethodType.Getter)]
    [HarmonyPostfix]
    private static void EliteGetter(ref bool __result)
    {
        __result = Enabled;
    }
}
