using KogamaTools.Behaviours;
using KogamaTools.Config;

namespace KogamaTools.Tools.Graphics;
[Section("Graphics")]
internal static class ClipPlaneModifier
{
    [Bind] internal static float NearClipPlane = UnityEngine.Camera.main.nearClipPlane;
    [Bind] internal static float FarClipPlane = UnityEngine.Camera.main.farClipPlane;

    [InvokeOnInit]
    internal static void ApplyChanges()
    {
        UnityEngine.Camera.main.nearClipPlane = NearClipPlane;
        UnityEngine.Camera.main.farClipPlane = FarClipPlane;
    }
}
