namespace KogamaTools.Tools.Graphics;
internal static class OrtographicCamera
{
    internal static bool Enabled = false;
    internal static float Size = 10f;

    internal static void ApplyChanges()
    {
        MVGameControllerBase.MainCameraManager.MainCamera.orthographic = Enabled;
        MVGameControllerBase.MainCameraManager.SecondaryCamera.orthographic = Enabled;
        MVGameControllerBase.MainCameraManager.TertiaryCamera.orthographic = Enabled;

        MVGameControllerBase.MainCameraManager.MainCamera.orthographicSize = Size;
        MVGameControllerBase.MainCameraManager.SecondaryCamera.orthographicSize = Size;
        MVGameControllerBase.MainCameraManager.TertiaryCamera.orthographicSize = Size;
    }
}
