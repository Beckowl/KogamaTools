using HarmonyLib;

namespace KogamaTools.Tools.PVP;

[HarmonyPatch]
internal static class AntiAFK
{
    internal static bool Enabled = false;

    [HarmonyPatch(typeof(AwayMonitor), "Update")]
    [HarmonyPrefix]
    private static bool Update()
    {
        return !Enabled;
    }
}
