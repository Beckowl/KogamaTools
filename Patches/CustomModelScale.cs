using System;
using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.patches
{
    [HarmonyPatch(typeof(EditorWorldObjectCreation))]
    internal static class CustomModelScale
    {
        public static Single CustomScale = ConfigHelper.GetConfigValue<Single>("CustomScale");
        public static bool Enabled = ConfigHelper.GetConfigValue<bool>("CustomScaleEnabled");

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