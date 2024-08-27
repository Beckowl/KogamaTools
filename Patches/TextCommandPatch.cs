using HarmonyLib;
using KogamaTools.Command;

namespace KogamaTools.Patches;

[HarmonyPatch(typeof(TextCommand))]
internal static class TextCommandPatch
{
    [HarmonyPatch("Resolve")]
    [HarmonyPrefix]
    private static bool Resolve(ref string commandLine)
    {
        TextCommand.Command command = commandLine;

        if (CommandHandler.TryExecuteCommand(commandLine))
        {
            return false;
        }
        return true;
    }
}