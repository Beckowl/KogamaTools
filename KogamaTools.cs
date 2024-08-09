﻿using BepInEx;
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
#pragma warning disable CS8618
        internal static ManualLogSource mls;
#pragma warning restore CS861

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
