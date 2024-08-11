using UnityEngine;

namespace KogamaTools.Behaviours
{
    internal class CameraFocus : MonoBehaviour
    {
        public CameraFocus(IntPtr handle) : base(handle) { }

        internal static CameraFocus instance;
        internal bool enabled = true;
        private float sensitivityMultiplier = 0.2f;
        private float zoomSpeed = 5f;
        private float originalFOV = 60f;
        private float targetFOV = 30f;
        private float originalSensitivity; // is this actually needed? idk but i'm storing it just in case
        private bool isZooming = false;
        private float currentVelocity = 0f;

        

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

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
            float targetValue = isZooming ? targetFOV : originalFOV;
            MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView = Mathf.SmoothDamp(
                MVGameControllerBase.MainCameraManager.MainCamera.fieldOfView,
                targetValue,
                ref currentVelocity,
                1 / zoomSpeed);
        }
    }
}
