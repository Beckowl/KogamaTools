using KogamaTools.Behaviours;
using KogamaTools.Config;

namespace KogamaTools.Tools.Graphics;

[Section("Graphics")]
internal static class ShadowDistModifier
{
    [Bind]
    internal static float ShadowDistance = UnityEngine.QualitySettings.shadowDistance;

    [InvokeOnInit]
    internal static void ApplyChanges()
    {
        UnityEngine.QualitySettings.shadowDistance = ShadowDistance;
    }
}
