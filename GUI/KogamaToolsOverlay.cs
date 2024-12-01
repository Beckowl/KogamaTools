using System.Runtime.InteropServices;
using ClickableTransparentOverlay;
using Il2CppInterop.Runtime;
using ImGuiNET;
using UnityEngine;
using KogamaTools.Behaviours;
using KogamaTools.GUI.Menus;

namespace KogamaTools.GUI;

internal class KogamaToolsOverlay : Overlay
{
    internal static bool ShouldRenderOverlay = true;
    private const int DefaultWidth = 416;
    private const int DefaultHeight = 300;
    private static readonly System.Numerics.Vector2 WindowSize = new System.Numerics.Vector2(DefaultWidth, DefaultHeight);

    private readonly string _windowName;

    public KogamaToolsOverlay(string windowName) : base(windowName, true)
    {
        _windowName = windowName;
    }

    protected override Task PostInitialized()
    {
        IL2CPP.il2cpp_thread_attach(IL2CPP.il2cpp_domain_get());

        HotkeySubscriber.Subscribe(KeyCode.F1, ToggleOverlay);

        VSync = true;
        Size = new System.Drawing.Size(DefaultWidth, DefaultHeight);
        return Task.CompletedTask;
    }

    protected override void Render()
    {
        if (!(ShouldRenderOverlay && IsGameFocused()))
        {
            return;
        }

        ImGui.Begin($"{KogamaTools.ModName} v{KogamaTools.ModVersion}");
        ImGui.SetWindowSize(WindowSize, ImGuiCond.FirstUseEver);

        if (ImGui.BeginTabBar("TabBar"))
        {
            BuildMenu.Render();
            PVPMenu.Render();
            GraphicsMenu.Render();
            InfoMenu.Render();

            ImGui.EndTabBar();
        }
        ImGui.End();
    }

    private void ToggleOverlay()
    {
        ShouldRenderOverlay = !ShouldRenderOverlay;
    }


    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string? className, string windowName);

    private bool IsGameFocused()
    {
        IntPtr foregroundWindow = GetForegroundWindow();
        return foregroundWindow == FindWindow(null, "KoGaMa") || foregroundWindow == FindWindow(null, _windowName);
    }

}
