using UnityEngine;

namespace KogamaTools.Behaviours;

internal class HotkeySubscriber : MonoBehaviour
{
    private static Dictionary<KeyCode, Action> events = new Dictionary<KeyCode, Action>();

    internal static void Subscribe(KeyCode key, Action action)
    {
        if (events.ContainsKey(key))
        {
            events[key] += action;
        }
        else
        {
            events[key] = action;
        }
    }

    /*
    internal static void Unsubscribe(KeyCode key, Action action)
    {
        if (events.ContainsKey(key))
        {
            events[key] -= action;
            if (events[key] == null)
            {
                events.Remove(key);
            }
        }
    }
    */

    private void Update()
    {
        foreach (var hotkey in events)
        {
            if (MVInputWrapper.DebugGetKeyDown(hotkey.Key))
            {
                hotkey.Value?.Invoke();
            }
        }
    }
}
