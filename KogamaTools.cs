using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using KogamaTools.Behaviours;
using KogamaTools.GUI;
using KogamaTools.Tools.Build;
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
    ModVersion = "0.2.2"; // TODO: automate this

    private readonly Harmony harmony = new Harmony(ModGUID);
    internal static ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource(ModGUID);
    internal static KogamaToolsOverlay Overlay = new KogamaToolsOverlay(ModName);

    public override void Load()
    {
        harmony.PatchAll();

        AddComponent<GameInitChecker>();

        GameInitChecker.OnGameInitialized += ObjectGrouper.OnGameInitialized;
        GameInitChecker.OnGameInitialized += GreetingMessage.JoinNotification;
        GameInitChecker.OnGameInitialized += ModelExporter.Init;
        GameInitChecker.OnGameInitialized += () =>
        {
            AddComponent<ModelImporter>();
            AddComponent<OverlayHotkeyListener>();
            AddComponent<UnityMainThreadDispatcher>();
            AddComponent<FOVModifier.FocusBehaviour>();
            AddComponent<LinkFix>();
            AddComponent<CopyPasteModel>();
            AddComponent<ObjectGrouper>();
            AddComponent<GameMetricsUpdater>();

            Application.quitting += (Action)(() => { Overlay.Close(); });
            Task.Run(Overlay.Start().Wait);
        };

        mls.LogInfo("KogamaTools isloaded, yay!");
    }
}