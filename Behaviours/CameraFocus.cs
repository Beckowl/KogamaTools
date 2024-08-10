using UnityEngine;

namespace KogamaTools.Behaviours
{
    // TODO: Add a command for this??? not sure
    internal class CameraFocus : MonoBehaviour
    {
        public CameraFocus(IntPtr handle) : base(handle) { }

        private float sensitivityMultiplier = 0.2f;
        private float zoomSpeed = 5f;
        private float originalFOV = 60;
        private float targetFOV = 30f;
        private float originalSensitivity; // is this actually needed? idk but i'm storing it just in case
        private bool isZooming = false;

        private void Update()
        {
            //KogamaTools.mls.LogInfo($"Game FOV: {MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView}");
            //KogamaTools.mls.LogInfo($"Mouse Sensitivity: {MVInputWrapper.MouseSensitivityModifier}");
            if (MVGameControllerBase.Game != null && MVGameControllerBase.Game.IsPlaying)
            {
                HandleFocus();
                DoZoom();
            }
        }

        private void HandleFocus()
        {
            KogamaTools.mls.LogInfo(MVInputWrapper.MouseSensitivityModifier);
            if (MVInputWrapper.GetBooleanControlDown(KogamaControls.PointerSelectAlt))
            {
                originalSensitivity = MVInputWrapper.MouseSensitivityModifier;

                MVInputWrapper.MouseSensitivityModifier *= sensitivityMultiplier;
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
            if (isZooming)
            {
                MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView = Mathf.Lerp(
                    MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView,
                    targetFOV,
                    Time.deltaTime * zoomSpeed);
            }
            else
            {
                MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView = Mathf.Lerp(
                    MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView,
                    originalFOV,
                    Time.deltaTime * zoomSpeed);
            }
        }
    }
}
