using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using KogamaTools.Behaviours;
using KogamaTools.GUI;
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
    internal static KogamaToolsOverlay overlay = new KogamaToolsOverlay(ModName);
    internal static KogamaTools? Instance;

    public override void Load()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        harmony.PatchAll();

        AddComponent<OverlayHotkeyListener>();
        AddComponent<UnityMainThreadDispatcher>();


        Application.quitting += (Action)(() => { overlay.Close(); });
        Task.Run(overlay.Start().Wait);

        mls.LogInfo("KogamaTools isloaded, yay!");
    }
}
