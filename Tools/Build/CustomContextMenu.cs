using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal class CustomContextMenu : MonoBehaviour
{
    private static readonly List<ContextMenuItem> menuItems = new List<ContextMenuItem>();

    internal static void AddButton(Func<MVWorldObjectClient, bool> condition, string buttonName, Action<ContextMenuController> action)
    {
        menuItems.Add(new ContextMenuItem(condition, buttonName, action));
    }

    internal static void AddButton(string buttonName, Action<ContextMenuController> action)
    {
        menuItems.Add(new ContextMenuItem((wo) => true, buttonName, action));
    }

    [HarmonyPatch(typeof(ContextMenuController), "PopGizmos")]
    [HarmonyPrefix]
    private static void PopGizmos(ContextMenuController __instance)
    {
        var wo = __instance.selectedWorldObject;
        if (wo == null) return;

        foreach (var item in menuItems)
        {
            if (item.Condition(wo))
            {
                CreateMenuItem(item.ButtonName, () => item.Action(__instance));
            }
        }
    }

    private static ContextMenu? FindContextMenu()
    {
        var menus = FindObjectsOfType<ContextMenu>();

        ContextMenu? menu = menus.FirstOrDefault();

        if (menu == null) return null;

        return menu;
    }

    private static void CreateMenuItem(string buttonName, Action onClick)
    {
        ContextMenu? menu = FindContextMenu();

        if (menu != null)
        {
            menu.AddButton(buttonName, onClick + menu.Pop);
        }
    }

    private class ContextMenuItem
    {
        public Func<MVWorldObjectClient, bool> Condition { get; }
        public string ButtonName { get; }
        public Action<ContextMenuController> Action { get; }

        public ContextMenuItem(Func<MVWorldObjectClient, bool> condition, string buttonName, Action<ContextMenuController> action)
        {
            Condition = condition;
            ButtonName = buttonName;
            Action = action;
        }
    }
}
