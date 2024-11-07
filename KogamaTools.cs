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
    ModVersion = "0.1.0";

    private readonly Harmony harmony = new Harmony(ModGUID);
    internal static ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource(ModGUID);
    internal static KogamaToolsOverlay Overlay = new KogamaToolsOverlay(ModName);

    public override void Load()
    {
        harmony.PatchAll();

        AddComponent<OverlayHotkeyListener>();
        AddComponent<UnityMainThreadDispatcher>();
        AddComponent<FOVModifier.FocusBehaviour>();
        AddComponent<LinkFix>();
        AddComponent<ObjectGrouper>();
        AddComponent<GameInitChecker>();

        GameInitChecker.OnGameInitialized += GreetingMessage.JoinNotification;
        GameInitChecker.OnGameInitialized += () =>
        {
            Application.quitting += (Action)(() => {Overlay.Close(); });
            Task.Run(Overlay.Start().Wait);
        };

        mls.LogInfo("KogamaTools isloaded, yay!");
    }
}