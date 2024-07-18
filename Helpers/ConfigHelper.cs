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

            BindConfig("BUILD MODE", "FastLinksEnabled", false, "");

            BindConfig("BUILD MODE", "MovementConstraintEnabled", true, "");

            BindConfig("BUILD MODE", "NoLimitEnabled", false, "");

            BindConfig("BUILD MODE", "RotationStepEnabled", false, "");
            BindConfig("BUILD MODE", "RotationStep", 15f, "");

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

            // KEYBINDS

            BindConfig("KEYBINDS", "KeybindsEnabled", false, "");

            BindConfig("KEYBINDS", "MoveForward", KeyCode.W, "");
            BindConfig("KEYBINDS", "MoveLeft", KeyCode.A, "");
            BindConfig("KEYBINDS", "MoveRight", KeyCode.D, "");
            BindConfig("KEYBINDS", "MoveBackwards", KeyCode.S, "");
            BindConfig("KEYBINDS", "AlternateCameraControls", KeyCode.W, "");
            BindConfig("KEYBINDS", "PointerSelect", KeyCode.W, "");
            BindConfig("KEYBINDS", "PointerSelectAlt", KeyCode.W, "");
            BindConfig("KEYBINDS", "EnterObject", KeyCode.W, "");
            BindConfig("KEYBINDS", "DeleteObject", KeyCode.W, "");
            BindConfig("KEYBINDS", "LeaveObject", KeyCode.W, "");
            BindConfig("KEYBINDS", "AddToSelection", KeyCode.W, "");
            BindConfig("KEYBINDS", "MoveDrawPlaneUp", KeyCode.W, "");
            BindConfig("KEYBINDS", "MoveDrawPlaneDown", KeyCode.W, "");
            BindConfig("KEYBINDS", "EmbedChangeGame", KeyCode.W, "");
            BindConfig("KEYBINDS", "ToggleFullScreen", KeyCode.W, "");
            BindConfig("KEYBINDS", "ShowChat", KeyCode.W, "");
            BindConfig("KEYBINDS", "Respawn", KeyCode.W, "");
            BindConfig("KEYBINDS", "TogglePlayerParticles", KeyCode.W, "");
            BindConfig("KEYBINDS", "ShowPlayerWindow", KeyCode.W, "");
            BindConfig("KEYBINDS", "DropCurrentItem", KeyCode.W, "");
            BindConfig("KEYBINDS", "Use", KeyCode.W, "");
            BindConfig("KEYBINDS", "FocusOnSelectedModel", KeyCode.W, "");
            BindConfig("KEYBINDS", "TogglePlayInEditor", KeyCode.W, "");
            BindConfig("KEYBINDS", "ToggleLogicRendering", KeyCode.W, "");
            BindConfig("KEYBINDS", "ToggleGripdSnapSize", KeyCode.W, "");
            BindConfig("KEYBINDS", "ActivateEditCubeTool", KeyCode.W, "");
            BindConfig("KEYBINDS", "ActivateDeleteCubeTool", KeyCode.W, "");
            BindConfig("KEYBINDS", "ActivetaPaintCubeTool", KeyCode.W, "");
            BindConfig("KEYBINDS", "ChangeMaterial", KeyCode.W, "");
            BindConfig("KEYBINDS", "OpenInventory", KeyCode.W, "");
            BindConfig("KEYBINDS", "CreateNewModel", KeyCode.W, "");
            BindConfig("KEYBINDS", "ToggleDrawPlane", KeyCode.W, "");
            BindConfig("KEYBINDS", "Fire", KeyCode.W, "");
            BindConfig("KEYBINDS", "Jump", KeyCode.W, "");
            BindConfig("KEYBINDS", "DrawAudioBox", KeyCode.W, "");
            BindConfig("KEYBINDS", "ChatSendLine", KeyCode.W, "");
            BindConfig("KEYBINDS", "ChatShiftLineUp", KeyCode.W, "");
            BindConfig("KEYBINDS", "ChatShiftLineDown", KeyCode.W, "");
            BindConfig("KEYBINDS", "ChangeFocus", KeyCode.W, "");
            BindConfig("KEYBINDS", "ChangeChangeFocusDirection", KeyCode.W, "");
            BindConfig("KEYBINDS", "ToggleHD", KeyCode.W, "");
            BindConfig("KEYBINDS", "LobbyMenu", KeyCode.W, "");
            BindConfig("KEYBINDS", "Escape", KeyCode.W, "");
            BindConfig("KEYBINDS", "Holster", KeyCode.W, "");
            BindConfig("KEYBINDS", "EditMoveUp", KeyCode.W, "");
            BindConfig("KEYBINDS", "EditMoveDown", KeyCode.W, "");
            BindConfig("KEYBINDS", "EditMoveForward", KeyCode.W, "");
            BindConfig("KEYBINDS", "EditMoveLeft", KeyCode.W, "");
            BindConfig("KEYBINDS", "EditMoveRight", KeyCode.W, "");
            BindConfig("KEYBINDS", "EditMoveBackwards", KeyCode.W, "");
            BindConfig("KEYBINDS", "EditMoveFast", KeyCode.W, "");
            BindConfig("KEYBINDS", "NotificationAcceptFriendshipRequest", KeyCode.W, "");
            BindConfig("KEYBINDS", "Size", KeyCode.W, "");

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
