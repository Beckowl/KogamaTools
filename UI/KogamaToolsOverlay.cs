using ImGuiNET;
using ClickableTransparentOverlay;
using System.Runtime.InteropServices;
using System.Numerics;
using KogamaTools.UI.Headers;
using Il2CppInterop.Runtime;

namespace KogamaTools
{
    public class KogamaToolsOverlay : Overlay
    {
        private const float DefaultWidth = 400f;
        private const float DefaultHeight = 300f;
        private static readonly Vector2 WindowSize = new Vector2(DefaultWidth, DefaultHeight);

        private readonly string _windowName;

        public KogamaToolsOverlay(string windowName) : base(windowName)
        {
            _windowName = windowName;
        }

        protected override Task PostInitialized()
        {
            VSync = true;
            return Task.CompletedTask;
        }

        protected override void Render()
        {
            if (!ShouldRenderOverlay())
            {
                return;
            }

            ImGui.Begin(KogamaTools.ModName, ImGuiWindowFlags.NoResize);
            ImGui.SetWindowSize(WindowSize);

            BuildHeader.Render();

            ImGui.End();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string? className, string windowName);

        private bool ShouldRenderOverlay()
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            return foregroundWindow == FindWindow(null, "KoGaMa") || foregroundWindow == FindWindow(null, _windowName);
        }

    }
}
