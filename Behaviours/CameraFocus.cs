using UnityEngine;

namespace KogamaTools.Behaviours;

internal class CameraFocus : MonoBehaviour
{
    public CameraFocus(IntPtr handle) : base(handle) { }

    internal static bool Enabled = true;
    internal static bool OverrideRailGunZoom = false;
    internal static float SensitivityMultiplier = 0.2f;
    internal static bool CustomFOVEnabled = false;
    internal static float CustomFOV = 60;
    internal static float FOVMultiplier = 0.5f;
    internal static float ZoomSpeed = 5f;

    private float originalSensitivity;
    private float zoomVelocity = 0f;
    private bool isZooming = false;

    private void Update()
    {
        if (MVGameControllerBase.Game != null && MVGameControllerBase.Game.IsPlaying)
        {
            HandleFocus();
            DoZoom();
        }
    }

    private void HandleFocus()
    {
        if (!Enabled)
        {
            return;
        }

        if (MVInputWrapper.GetBooleanControlDown(KogamaControls.PointerSelectAlt))
        {
            originalSensitivity = MVInputWrapper.MouseSensitivityModifier;
            MVInputWrapper.MouseSensitivityModifier *= SensitivityMultiplier;
            isZooming = true;
        }
        else if (MVInputWrapper.GetBooleanControlUp(KogamaControls.PointerSelectAlt))
        {
            MVInputWrapper.MouseSensitivityModifier = originalSensitivity;
            isZooming = false;
        }
    }

    private void DoZoom()
    {
        float originalFOV = CustomFOVEnabled ? CustomFOV : 60f;
        float targetValue = isZooming ? originalFOV * FOVMultiplier : originalFOV;

        MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView = Mathf.SmoothDamp(
            MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView,
            targetValue,
            ref zoomVelocity,
            1 / ZoomSpeed);
    }
}
