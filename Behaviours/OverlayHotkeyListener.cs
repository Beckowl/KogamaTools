using KogamaTools.GUI;
using UnityEngine;

namespace KogamaTools.Behaviours
{
    internal class OverlayHotkeyListener : MonoBehaviour
    {
        public OverlayHotkeyListener(IntPtr handle) : base(handle) { }

        private void Update()
        {
            if (MVInputWrapper.DebugGetKeyDown(KeyCode.F1))
                {
                    KogamaToolsOverlay.ShouldRenderOverlay = !KogamaToolsOverlay.ShouldRenderOverlay;
                }
        }
    }
}
