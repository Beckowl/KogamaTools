using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using KogamaTools.Behaviours;
using KogamaTools.Config;
using KogamaTools.GUI;
using KogamaTools.Tools.Build;
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
    internal static KogamaToolsOverlay Overlay = new KogamaToolsOverlay($"{KogamaTools.ModName} v{KogamaTools.ModVersion}");
    internal static AutoConfigManager ConfigManager = null!;

    private static ConfigFile configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, $"{ModName}.cfg"), true);

    private readonly Harmony harmony = new Harmony(ModGUID);

    public override void Load()
    {
        harmony.PatchAll();

        AddComponent<GameInitChecker>();

        GameInitChecker.OnGameInitialized += () =>
        {
            configFile.SaveOnConfigSet = true;

            ConfigManager = new AutoConfigManager(configFile);
            ConfigManager.LoadValuesToFields();

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