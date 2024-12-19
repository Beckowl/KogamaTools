using HarmonyLib;

namespace KogamaTools.Tools.PVP;

[HarmonyPatch]
internal static class ForceThirdPerson
{
    internal static bool Enabled = false; // for the GUI
    private static bool enabledInternal = Enabled;

    [HarmonyPatch(typeof(PickupItem), nameof(PickupItem.FirstPerson), MethodType.Getter)]
    [HarmonyPostfix]
    private static void FirstPersonGetter(ref bool __result)
    {
        __result ^= enabledInternal;
    }

    [HarmonyPatch(typeof(PickupItem), "OnEquip")]
    [HarmonyPatch(typeof(PickupItem), "OnHolstered")]
    [HarmonyPostfix]
    private static void OnEquip()
    {
        enabledInternal = Enabled;
    }
}
