using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using KogamaTools.Behaviours;
using KogamaTools.GUI;
using KogamaTools.Helpers;
using KogamaTools.Tools.Build;
using KogamaTools.Tools.Graphics;
using KogamaTools.Tools.Misc;
using KogamaTools.Tools.PVP;
using UnityEngine;

namespace KogamaTools;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class KogamaTools : BasePlugin
{
    public const string
    ModGUID = "KogamaTools",
    ModName = "KogamaTools",
    ModVersion = "0.8.1"; // TODO: automate this

    private readonly Harmony harmony = new Harmony(ModGUID);
    internal static ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource(ModGUID);
    internal static KogamaToolsOverlay Overlay = new KogamaToolsOverlay(ModName);

    public override void Load()
    {
        harmony.PatchAll();

        AddComponent<GameInitChecker>();

        GameInitChecker.OnGameInitialized += RuntimeReferences.LoadReferences;
        GameInitChecker.OnGameInitialized += MouseColorPick.SubscribeHotkeys;
        GameInitChecker.OnGameInitialized += ModelExporter.Init;
        GameInitChecker.OnGameInitialized += GreetingMessage.JoinNotification;
        GameInitChecker.OnGameInitialized += ConsoleToggle.SubscribeHotkeys;
        GameInitChecker.OnGameInitialized += ScreenshotUtil.SubscribeHotkeys;

        GameInitChecker.OnGameInitialized += () =>
        {
            AddComponent<UnityMainThreadDispatcher>();
            AddComponent<HotkeySubscriber>();
            AddComponent<GameMetricsUpdater>();
            AddComponent<ModelImporter>();
            AddComponent<CameraFocus>();
            //AddComponent<FOVModifier.FocusBehaviour>();
            AddComponent<LinkFix>();
            AddComponent<CopyPasteModel>();
            AddComponent<ObjectGrouper>();


            Application.quitting += (Action)(() => { Overlay.Close(); });
            Task.Run(Overlay.Start().Wait);
        };

        mls.LogInfo("KogamaTools isloaded, yay!");
    }
}