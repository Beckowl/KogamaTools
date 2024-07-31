using System.Collections.Generic;
using BepInEx;
using System.IO;
using BepInEx.Configuration;
using UnityEngine;
namespace KogamaTools.Helpers
{
    internal static class ConfigHelper
    {
        private static ConfigFile configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, $"{KogamaTools.ModName}.cfg"), true);
        private static Dictionary<string, object> configEntries = new Dictionary<string, object>();

        private static void BindConfig<T>(string section, string key, T defaultValue, string description)
        {
            ConfigEntry<T> entry = configFile.Bind(section, key, defaultValue, description);
            configEntries[key] = entry;
        }

        // TODO: MOVE CONFIG DEFAULTS TO A SEPARATE FILE ASAP
        // Define default keybinds
        public static void BindConfigs()
        {
            // BUILD MODE

            BindConfig("BUILD MODE", "BlueModeEnabled", true, "");
            BindConfig("BUILD MODE", "CustomScaleEnabled", false, "");
            BindConfig("BUILD MODE", "CustomScale", 1.0f, "");
            BindConfig("BUILD MODE", "CustomGridEnabled", false, "");
            BindConfig("BUILD MODE", "GridSize", 1.0f, "");
            BindConfig("BUILD MODE", "FastLinksEnabled", false, "");
            BindConfig("BUILD MODE", "MovementConstraintEnabled", true, "");
            BindConfig("BUILD MODE", "NoLimitEnabled", false, "");
            BindConfig("BUILD MODE", "RotationStepEnabled", false, "");
            BindConfig("BUILD MODE", "RotationStep", 15f, "");
            BindConfig("BUILD MODE", "SingleSidePaintingEnabled", false, "");
            BindConfig("BUILD MODE", "SpeedMultEnabled", false, "");
            BindConfig("BUILD MODE", "SpeedMult", 1f, "");
            BindConfig("BUILD MODE", "UnlimitedConfigEnabled", false, "");
            BindConfig("BUILD MODE", "MinValue", 0f, "");
            BindConfig("BUILD MODE", "MaxValue", 1f, "");

            // PVP

            BindConfig("GRAPHICS", "AntiAFKEnabled", false, "");
            BindConfig("PVP", "CustomFOVEnabled", false, "");
            BindConfig("PVP", "FOV", 80f, "");

            // GRAPHICS
            /*
            BindConfig("GRAPHICS", "FogEnabled", true, "");
            BindConfig("GRAPHICS", "FogType", 0, "");
            BindConfig("GRAPHICS", "CustomFogColor", false, "");
            BindConfig("GRAPHICS", "FogR", 1f, "");
            BindConfig("GRAPHICS", "FogG", 1f, "");
            BindConfig("GRAPHICS", "FogB", 1f, "");

            BindConfig("GRAPHICS", "OrtographicCamera", false, "");
            BindConfig("GRAPHICS", "OrtographicCameraSize", 10f, "");

            BindConfig("GRAPHICS", "RenderDistance", 600f, "");

            BindConfig("GRAPHICS", "ShadowDistance", 10f, "");

            BindConfig("GRAPHICS", "ThemesEnabled", false, "");
            BindConfig("GRAPHICS", "ForceThemes", false, "");
            BindConfig("GRAPHICS", "ThemeID", 0, "");
            */
            // MISC

            BindConfig("MISC", "ShowGreetingMessage", true, "");
        }

        public static ConfigEntry<T> GetConfig<T>(string key)
        {
            if (configEntries.TryGetValue(key, out var entry))
            {
                return entry as ConfigEntry<T>;
            }

            throw new KeyNotFoundException($"Config entry \"{key}\" not found.");
        }

        public static T GetConfigValue<T>(string key)
        {
            ConfigEntry<T> configEntry = GetConfig<T>(key);
            return configEntry.Value;
        }
    }
}
