using HarmonyLib;
using Il2CppInterop.Runtime;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
internal static class ChatToggle
{
    internal static bool ChatVisible = true;
    internal static void UpdateChatVisibility()
    {
        UnityEngine.Object[] chatControllers = UnityEngine.Object.FindObjectsOfType(Il2CppType.Of<ChatControllerUGUI>(), true);

        foreach (var chatController in chatControllers)
        {
            chatController.Cast<ChatControllerUGUI>().gameObject.active = ChatVisible;
        }
    }
}
