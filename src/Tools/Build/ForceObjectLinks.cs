using HarmonyLib;
using WorldObjectTypes.MVObjectTransparency;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class ForceObjectLinks
{
    internal static bool Enabled = false;

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
