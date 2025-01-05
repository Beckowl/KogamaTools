using HarmonyLib;
using KogamaTools.Config;

namespace KogamaTools.Tools.PVP;

[HarmonyPatch]
[Section("PVP")]
internal static class AntiAFK
{
    [Bind] internal static bool Enabled = false;

    [HarmonyPatch(typeof(AwayMonitor), "Update")]
    [HarmonyPrefix]
    private static bool Update()
    {
        return !Enabled;
    }
}
