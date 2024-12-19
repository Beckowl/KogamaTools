using HarmonyLib;

namespace KogamaTools.Tools.PVP;

[HarmonyPatch]
internal static class ForceThirdPersonCamera
{
    internal static bool Enabled = false;
    private static bool enabledInternal = Enabled;

    [HarmonyPatch(typeof(PickupItem), nameof(PickupItem.FirstPerson), MethodType.Getter)]
    [HarmonyPostfix]
    private static void FirstPersonGetter(ref bool __result)
    {
        if (enabledInternal)
        {
            __result = false;
        }
    }

    [HarmonyPatch(typeof(PickupItem), "OnEquip")]
    [HarmonyPatch(typeof(PickupItem), "OnHolstered")]
    [HarmonyPostfix]
    private static void OnEquip()
    {
        enabledInternal = Enabled;
    }
}
