using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using KogamaTools.Behaviours;
using KogamaTools.Command;
using KogamaTools.GUI;
using KogamaTools.Helpers;
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

    public override void Load()
    {
        ConfigHelper.BindConfigs();
        CommandHandler.LoadCommands();

        harmony.PatchAll();

        AddComponent<CameraFocus>();
        AddComponent<OverlayHotkeyListener>();
        AddComponent<UnityMainThreadDispatcher>();
        AddComponent<ObjectGrouper>();

        Application.quitting += (Action)(() => { overlay.Close(); });
        Task.Run(overlay.Start().Wait);

        mls.LogInfo("KogamaTools isloaded, yay!");
    }
}
