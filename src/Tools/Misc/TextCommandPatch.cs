using HarmonyLib;
using KogamaTools.Command;

namespace KogamaTools.Tools.Misc;

[HarmonyPatch]
internal static class TextCommandPatch
{
    [HarmonyPatch(typeof(TextCommand), "Resolve")]
    [HarmonyPrefix]
    private static bool Resolve(string commandLine)
    {
        if (CommandHandler.TryExecuteCommand(commandLine))
        {
            return false;
        }

        return true;
    }
}