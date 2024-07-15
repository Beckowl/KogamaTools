using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using KogamaTools.Command;
using KogamaTools.Helpers;

namespace KogamaTools
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class KogamaTools : BasePlugin
    {

        public const string
        ModGUID = "KogamaTools",
        ModName = "KogamaTools",
        ModVersion = "0.1.0";

        private readonly Harmony harmony = new Harmony(ModGUID);
        internal static ManualLogSource mls;
        public override void Load()
        {
            mls = Logger.CreateLogSource(ModGUID);

            ConfigHelper.BindConfigs();
            CommandHandler.LoadCommands();
            harmony.PatchAll();

            mls.LogInfo("KogamaTools isloaded, yay!");
        }
    }
}
