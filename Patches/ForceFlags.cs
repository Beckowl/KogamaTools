using HarmonyLib;

namespace KogamaTools.Patches
{
    internal static class ForceFlags
    {
        internal static ulong Flags = 0;

        internal static void AddFlags(InteractionFlags flag)
        {
            Flags |= (ulong)flag;
        }

        internal static void RemoveFlags(InteractionFlags flag)
        {
            Flags &= (ulong)~flag;
        }

        internal static void ToggleFlags(InteractionFlags flag)
        {
            Flags ^= (ulong)flag;
        }

        internal static bool AreFlagsSet(InteractionFlags flag)
        {
            return (Flags & (ulong)flag) == (ulong)flag;
        }

        [HarmonyPatch(typeof(MVWorldObjectClient), "HasInteractionFlag")]
        [HarmonyPostfix]
        private static void HasInteractionFlag(ref InteractionFlags flag, MVWorldObjectClient __instance, ref bool __result)
        {
            __result = (((ulong)__instance.interactionFlags | Flags) & (ulong)flag) == (ulong)flag;
        }
    }
}
