using KogamaTools.Behaviours;
using KogamaTools.Config;

namespace KogamaTools.Tools.Graphics;

[Section("Graphics")]
internal static class OrtographicCamera
{
    [Bind] internal static bool Enabled = false;
    [Bind] internal static float Size = 10f;

    [InvokeOnInit]
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
