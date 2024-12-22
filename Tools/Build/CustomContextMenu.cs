using HarmonyLib;
using KogamaTools.Helpers;
using UnityEngine.Events;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class CustomContextMenu
{
    private static readonly List<ContextMenuItem> menuItems = new List<ContextMenuItem>();

    internal static void AddButton(string buttonName, Func<MVWorldObjectClient, bool> condition, Action<MVWorldObjectClient> action)
    {
        menuItems.Add(new ContextMenuItem(condition, buttonName, action));
    }

    [HarmonyPatch(typeof(ContextMenu), "AddButton")]
    [HarmonyPrefix]
    private static bool AddButton(string buttonText, UnityAction onClickCallback, ContextMenu __instance)
    {
        MVWorldObjectClient wo = RuntimeReferences.DesktopEditModeController.contextMenuController.selectedWorldObject;

        if (buttonText != TM._("Info")) return true;

        foreach (var item in menuItems)
        {
            if (item.Condition(wo))
            {
                AddCustomButton(item.ButtonName, () => item.Action(wo), __instance);
            }
        }

        return (ForceFlags.Flags & InteractionFlags.Info) == InteractionFlags.Info && ForceFlags.Enabled;
    }

    private static void AddCustomButton(string buttonText, Action onClickCallback, ContextMenu __instance)
    {
        ContextMenuButton contextMenuButton = UnityEngine.Object.Instantiate(__instance.contextMenuButtonPrefab);
        contextMenuButton.Initialize(buttonText, onClickCallback + __instance.Pop);
        contextMenuButton.transform.SetParent(__instance.transform, false);
    }

    private class ContextMenuItem
    {
        public Func<MVWorldObjectClient, bool> Condition { get; }
        public string ButtonName { get; }
        public Action<MVWorldObjectClient> Action { get; }

        public ContextMenuItem(Func<MVWorldObjectClient, bool> condition, string buttonName, Action<MVWorldObjectClient> action)
        {
            Condition = condition;
            ButtonName = buttonName;
            Action = action;
        }
    }
}
