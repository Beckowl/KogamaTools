using UnityEngine;

namespace KogamaTools.Tools.Misc;
internal enum PlayControls
{
    DropCurrentItem = 0x13,
    Fire = 0x20,
    Holster = 0x2B,
    Jump = 0x21,
    MoveBackwards = 0x03,
    MoveForward = 0,
    MoveLeft = 0x01,
    MoveRight = 0x02,
    Respawn = 0x10,
    Use = 0x14
}
internal static class KeyRemapper
{
    internal static void RemapControl<T>(KogamaControls control, KeyCode key) where T : DesktopDefaultKeyboardMapping
    {
        T keymapping = MVInputWrapper.inputMap.Cast<T>();
        if (keymapping.keyMapping.ContainsKey(control))
        {
            keymapping.keyMapping[control] = new KeyCode[] { key };
        }
    }

    internal static void ResetToDefaults<T>(KogamaControls control) where T : DesktopDefaultKeyboardMapping
    {
        T keymapping = MVInputWrapper.inputMap.Cast<T>();
        if (keymapping.keyMapping.ContainsKey(control))
        {
            T defaultKeymap = Activator.CreateInstance<T>();
            keymapping.keyMapping[control] = defaultKeymap.keyMapping[control];
        }
    }

    internal static KeyCode GetKeyCodeForControl<T>(KogamaControls control) where T : DesktopDefaultKeyboardMapping
    {
        T keymapping = MVInputWrapper.inputMap.Cast<T>();
        if (keymapping.keyMapping.ContainsKey(control))
        {
            return keymapping.keyMapping[control].First();
        }
        return KeyCode.None;
    }
}
