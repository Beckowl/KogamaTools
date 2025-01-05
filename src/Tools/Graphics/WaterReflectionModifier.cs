using Assets.Scripts.ProfileSettings;
using HarmonyLib;
using KogamaTools.Behaviours;
using KogamaTools.Config;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
[Section("Graphics")]
internal static class WaterReflectionModifier
{
    [Bind] internal static bool UseReflectiveWater = GetWaterMode();

    static WaterReflectionModifier()
    {
        ApplyChanges();
    }

    [InvokeOnInit]
    internal static void ApplyChanges()
    {
        Water.WaterMode mode = UseReflectiveWater ? Water.WaterMode.Reflective : Water.WaterMode.Simple;

        MVGameControllerBase.WaterPlaneManager.water.waterMode = mode;
    }

    private static bool GetWaterMode()
    {
        if (!GameInitChecker.IsInitialized)
        {
            GameInitChecker.OnGameInitialized += () =>
            {
                UseReflectiveWater = GetWaterMode();
            };
        }
        return MVGameControllerBase.WaterPlaneManager.water.waterMode == Water.WaterMode.Reflective;
    }

    [HarmonyPatch(typeof(ProfileSettingsManager), "SetLightQualitySetting")]
    [HarmonyPostfix]
    private static void SetLightQualitySetting()
    {
        ApplyChanges();
    }
}
