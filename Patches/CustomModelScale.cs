using System;
using HarmonyLib;

namespace KogamaTools.patches
{
    [HarmonyPatch(typeof(EditorWorldObjectCreation))]
    internal static class CustomModelScale
    {
        public static Single CustomScale = 1f;
        public static bool Enabled = false;

        [HarmonyPatch("OnAddNewPrototype")]
        [HarmonyPrefix]
        static void OnAddNewPrototype(ref Single scale)
        {
            if (Enabled)
            {
                scale = CustomScale;
            }
        }
    }
}