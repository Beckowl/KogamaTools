using HarmonyLib;
using KogamaTools.Config;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
[Section("Build")]
internal static class ForceFlags
{
    [Bind] internal static bool Enabled = false;
    [Bind] internal static bool Override = false;
    internal static InteractionFlags Flags;

    internal static void AddFlags(InteractionFlags flag)
    {
        Flags |= flag;
    }

    internal static void RemoveFlags(InteractionFlags flag)
    {
        Flags &= flag;
    }

    internal static void ToggleFlags(InteractionFlags flag)
    {
        KogamaTools.mls.LogInfo(Flags);
        Flags ^= flag;
    }

    internal static bool AreFlagsSet(InteractionFlags flag)
    {
        return (Flags & flag) == flag;
    }

    [HarmonyPatch(typeof(MVWorldObjectClient), "HasInteractionFlag")]
    [HarmonyPostfix]
    private static void HasInteractionFlag(ref InteractionFlags flag, MVWorldObjectClient __instance, ref bool __result)
    {
        if (flag == InteractionFlags.Info)
        {
            __result = true;
            return;
        }

        if (!Enabled)
        {
            return;
        }
        __result = ((Flags | (Override ? InteractionFlags.None : __instance.interactionFlags)) & flag) == flag;
    }
}
