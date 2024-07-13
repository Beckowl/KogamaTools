using HarmonyLib;

namespace KogamaTools.patches
{
    [HarmonyPatch(typeof(TextCommand))]
    internal class TextCommandPatch
    {
        [HarmonyPatch("Resolve")]
        [HarmonyPrefix]
        static bool Resolve(ref string commandLine)
        {
            TextCommand.Command command = commandLine;
            string text = command.Name.ToLower();
            if (text != null)
            {
                if (text == "/abctest" || text == "/assetbundlecachetest")
                {
                    TextCommand.Command_AssetBundleCacheTest(command);
                    return false;
                }
                if (CommandHandler.TryExecuteCommand(commandLine))
                {
                    return false;
                }
            }
            TextCommand.Command_Invalid(command);
            return false;
        }
    }
}
