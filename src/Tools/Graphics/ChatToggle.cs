using HarmonyLib;
using Il2CppInterop.Runtime;
using KogamaTools.Behaviours;
using KogamaTools.Config;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
[Section("Graphics")]
internal static class ChatToggle
{
    [Bind] internal static bool ChatVisible = true;

    [InvokeOnInit]
    internal static void UpdateChatVisibility()
    {
        UnityEngine.Object[] chatControllers = UnityEngine.Object.FindObjectsOfType(Il2CppType.Of<ChatControllerUGUI>(), true);

        foreach (var chatController in chatControllers)
        {
            chatController.Cast<ChatControllerUGUI>().gameObject.active = ChatVisible;
        }
    }
}
