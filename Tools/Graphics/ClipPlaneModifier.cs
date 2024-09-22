namespace KogamaTools.Tools.Graphics;
internal class ClipPlaneModifier
{
    internal static float NearClipPlane = UnityEngine.Camera.main.nearClipPlane;
    internal static float FarClipPlane = UnityEngine.Camera.main.farClipPlane;

    internal static void ApplyClipPlane()
    {
        UnityEngine.Camera.main.nearClipPlane = NearClipPlane;
        UnityEngine.Camera.main.farClipPlane = FarClipPlane;
    }
}
