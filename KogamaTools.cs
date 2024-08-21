using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using KogamaTools.Behaviours;
using KogamaTools.GUI;
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
#pragma warning disable CS8618
        internal static ManualLogSource mls;
#pragma warning restore CS861
        internal static UnityMainThreadDispatcher unityMainThreadDispatcher;
        internal static KogamaToolsOverlay overlay = new KogamaToolsOverlay(ModName);

        public override void Load()
        {
            mls = Logger.CreateLogSource(ModGUID);

            ConfigHelper.BindConfigs();
            CommandHandler.LoadCommands();
            harmony.PatchAll();

            AddComponent<CameraFocus>();
            AddComponent<OverlayHotkeyListener>();

            Task.Run(overlay.Start().Wait);

            mls.LogInfo("KogamaTools isloaded, yay!");
        }
    }
}
