using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using ClickableTransparentOverlay;
using Il2CppInterop.Runtime;
using ImGuiNET;
using KogamaTools.Behaviours;
using KogamaTools.Config;
using KogamaTools.GUI.Menus;

namespace KogamaTools.GUI;

[Section("Misc")]
internal class KogamaToolsOverlay : Overlay
{
    [Bind] private static bool HideOverlay;
    [Bind] private static bool useCompatibilityMode; // dummy field so i can bind a config

    private static IntPtr handle;

    private const int defaultWidth = 345;
    private const int defaultHeight = 422;

    private const ImGuiWindowFlags compatibilityFlags = ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize;

    private readonly string _windowName;

    public KogamaToolsOverlay(string windowName) : base(windowName)
    {
        _windowName = windowName;
    }

    protected override Task PostInitialized()
    {
        IL2CPP.il2cpp_thread_attach(IL2CPP.il2cpp_domain_get());

        HotkeySubscriber.Subscribe(UnityEngine.KeyCode.F1, ToggleOverlay);
        handle = FindWindow(null, _windowName);

        VSync = true;
        CompatibilityMode = useCompatibilityMode;

        return Task.CompletedTask;
    }

    protected override void Render()
    {
        if (HideOverlay || !IsGameFocused()) return;

        ImGui.Begin(_windowName, CompatibilityMode ? compatibilityFlags : ImGuiWindowFlags.None);
        ImGui.SetWindowSize(new Vector2(defaultWidth, defaultHeight), ImGuiCond.FirstUseEver);

        if (ImGui.BeginTabBar("TabBar"))
        {
            BuildMenu.Render();
            PVPMenu.Render();
            GraphicsMenu.Render();
            ConfigMenu.Render();
            InfoMenu.Render();

            ImGui.EndTabBar();
        }

        if (CompatibilityMode)
        {
            if (ImGui.IsWindowAppearing())
            {
                Vector2 windowSize = ImGui.GetWindowSize();
                Size = new((int)windowSize.X, (int)windowSize.Y);

                ImGui.SetWindowPos(Vector2.Zero);
            }
            ImGui.SetWindowSize(new Vector2(Size.Width, Size.Height));
        }

        ImGui.End();
    }

    private void ToggleOverlay()
    {
        HideOverlay = !HideOverlay;
        if (CompatibilityMode)
        {
            ShowWindow(handle, HideOverlay ? 0 : 4); // Hide & ShowNoActivate
        }
    }

    private bool IsGameFocused()
    {
        IntPtr foregroundWindow = GetForegroundWindow();
        return foregroundWindow == FindWindow(null, "KoGaMa") || foregroundWindow == handle;
    }

    [DllImport("user32.dll", ExactSpelling = true)]
    internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    internal static extern IntPtr FindWindow(string? className, string windowName);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
}
