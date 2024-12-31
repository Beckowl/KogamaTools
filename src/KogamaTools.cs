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

[BepInPlugin(ModInfo.ModGUID, ModInfo.ModName, ModInfo.ModVersion)]
public class KogamaTools : BasePlugin
{
    internal static readonly string ModGUID = ModInfo.ModGUID;
    internal static readonly string ModName = ModInfo.ModName;
    internal static readonly string ModVersion = ModInfo.ModVersion;

    internal static ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource(ModName);
    internal static KogamaToolsOverlay Overlay = new KogamaToolsOverlay(ModName);

    private readonly Harmony harmony = new Harmony(ModGUID);
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
            AddComponent<CameraFocus>();
            AddComponent<ModelImporter>();
            AddComponent<MultiSelect>();
            AddComponent<LinkFix>();
            AddComponent<CopyPasteModel>();
            AddComponent<ObjectGrouper>();


            Application.quitting += (Action)(() => { Overlay.Close(); });
            Task.Run(Overlay.Start().Wait);
        };

        mls.LogInfo("KogamaTools isloaded, yay!");
    }
}