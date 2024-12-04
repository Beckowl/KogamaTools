namespace KogamaTools.Tools.Graphics;
internal static class ShadowDistModifier
{
    internal static float ShadowDistance = UnityEngine.QualitySettings.shadowDistance;

    internal static void ApplyChanges()
    {
        UnityEngine.QualitySettings.shadowDistance = ShadowDistance;
    }
}
