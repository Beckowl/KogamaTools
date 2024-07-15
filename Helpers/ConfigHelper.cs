using System.Collections.Generic;
using BepInEx;
using System.IO;
using BepInEx.Configuration;

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

        public static void BindConfigs()
        {
            BindConfig("Defaults", "BlueModeEnabled", true, "");

            BindConfig("Defaults", "CustomScaleEnabled", false, "");
            BindConfig("Defaults", "CustomScale", 1.0f, "");

            BindConfig("Defaults", "FastLinksEnabled", false, "");

            BindConfig("Defaults", "ShowGreetingMessage", true, "");

            BindConfig("Defaults", "NoLimitEnabled", false, "");

            BindConfig("Defaults", "RotationStepEnabled", false, "");
            BindConfig("Defaults", "RotationStep", 15f, "");

            BindConfig("Defaults", "SpeedMultEnabled", false, "");
            BindConfig("Defaults", "SpeedMult", 1f, "");
            BindConfig("Defaults", "MovementConstraintEnabled", true, "");
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
