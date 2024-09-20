using System.Runtime.InteropServices;
using ClickableTransparentOverlay;
using ImGuiNET;
using KogamaTools.GUI.Menus;

namespace KogamaTools.GUI;

internal class KogamaToolsOverlay : Overlay
{
    internal static bool ShouldRenderOverlay = true;
    private const float DefaultWidth = 416f;
    private const float DefaultHeight = 300f;
    private static readonly System.Numerics.Vector2 WindowSize = new System.Numerics.Vector2(DefaultWidth, DefaultHeight);


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
        if (!(ShouldRenderOverlay && IsGameFocused()))
        {
            return;
        }

        ImGui.Begin(KogamaTools.ModName, ImGuiWindowFlags.NoResize);
        ImGui.SetWindowSize(WindowSize, ImGuiCond.Always);

        if (ImGui.BeginTabBar("TabBar"))
        {
            BuildMenu.Render();
            PVPMenu.Render();
            GraphicsMenu.Render();

            ImGui.EndTabBar();
        }
        ImGui.End();
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
