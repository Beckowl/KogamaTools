namespace KogamaTools.Tools.Graphics;
internal static class OrtographicCamera
{
    internal static bool Enabled = false;
    internal static float Size = 10f;

    internal static void ApplyChanges()
    {
        MVGameControllerBase.MainCameraManager.mainCamera.orthographic = Enabled;
        MVGameControllerBase.MainCameraManager.mainCamera.orthographicSize = Size;
    }
}
