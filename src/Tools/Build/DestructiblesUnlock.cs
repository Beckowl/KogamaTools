using HarmonyLib;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class DestructiblesUnlock
{
    internal static bool Unlocked = false;

    [HarmonyPatch(typeof(MVMaterial), nameof(MVMaterial.IsAvailable), MethodType.Getter)]
    [HarmonyPostfix]
    private static void AllowDestructibleMaterialSelection(ref bool __result)
    {
        __result |= Unlocked;
    }
}
