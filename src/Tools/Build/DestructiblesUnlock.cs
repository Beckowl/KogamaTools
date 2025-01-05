using HarmonyLib;
using KogamaTools.Config;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
[Section("Build")]
internal static class DestructiblesUnlock
{
    [Bind] internal static bool Unlocked = false;

    [HarmonyPatch(typeof(MVMaterial), nameof(MVMaterial.IsAvailable), MethodType.Getter)]
    [HarmonyPostfix]
    private static void AllowDestructibleMaterialSelection(ref bool __result)
    {
        __result |= Unlocked;
    }
}
