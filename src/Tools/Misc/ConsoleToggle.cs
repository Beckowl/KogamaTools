using BepInEx;
using KogamaTools.Behaviours;

namespace KogamaTools.Tools.Misc;
internal static class ConsoleToggle
{
    [InvokeOnInit]
    internal static void SubscribeHotkeys()
    {
        HotkeySubscriber.Subscribe(UnityEngine.KeyCode.F12, ToggleConsole);
    }

    internal static void ToggleConsole()
    {
        if (ConsoleManager.ConsoleActive)
        {
            ConsoleManager.DetachConsole();
        }
        else
        {
            ConsoleManager.CreateConsole();
        }
    }
}
