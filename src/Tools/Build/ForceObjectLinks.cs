using HarmonyLib;
using KogamaTools.Config;
using WorldObjectTypes.MVObjectTransparency;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
[Section("Build")]
internal static class ForceObjectLinks
{
    [Bind] internal static bool Enabled = false;

    [HarmonyPatch(typeof(MVObjectEnabler), "ValidateObjectLinkTarget")]
    [HarmonyPatch(typeof(MVObjectTransparency), "ValidateObjectLinkTarget")]
    [HarmonyPostfix]
    private static void ValidateObjectLink(MVWorldObjectClient wo, ref bool __result)
    {
        if (Enabled)
        {
            __result = wo.InteractionFlags.HasFlag(InteractionFlags.HasCubeModel);
        }
    }
}
