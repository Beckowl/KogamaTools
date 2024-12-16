using HarmonyLib;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
internal static class CustomModelScale
{
    internal static bool Enabled = false;
    internal static float Scale = 4;

    [HarmonyPatch(typeof(EditorWorldObjectCreation), "OnAddNewPrototype")]
    [HarmonyPrefix]
    private static void OnAddNewPrototype(ref float scale)
    {
        if (Enabled)
        {
            scale = Scale;
        }
    }
}