using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class CustomModelScale
{
    internal static float CustomScale = 4;
    internal static bool Enabled = false;

    [HarmonyPatch(typeof(EditorWorldObjectCreation), "OnAddNewPrototype")]
    [HarmonyPrefix]
    private static void OnAddNewPrototype(ref float scale)
    {
        if (Enabled)
        {
            scale = CustomScale;
        }
    }
}