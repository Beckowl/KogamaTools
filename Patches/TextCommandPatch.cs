using HarmonyLib;
using KogamaTools.Command;

namespace KogamaTools.Patches
{
    internal static class TextCommandPatch
    {
        [HarmonyPatch(typeof(TextCommand), "Resolve")]
        [HarmonyPrefix]
        private static bool Resolve(ref string commandLine)
        {
            if (CommandHandler.TryExecuteCommand(commandLine))
            {
                return false;
            }
            return true;
        }
    }
}
