using Assets.Scripts.ProfileSettings;
using HarmonyLib;
using KogamaTools.Behaviours;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
internal static class WaterReflectionModifier
{
    internal static bool UseReflectiveWater = GetWaterMode();

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
