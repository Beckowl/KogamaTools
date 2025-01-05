using HarmonyLib;
using KogamaTools.Config;

namespace KogamaTools.Tools.Build;

[HarmonyPatch]
[Section("Build")]
internal static class CustomModelScale
{
    [Bind] internal static bool Enabled = false;
    [Bind] internal static float Scale = 4;

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