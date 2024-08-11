﻿using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Patches
{
    internal static class CustomModelScale
    {
        internal static Single CustomScale = ConfigHelper.GetConfigValue<Single>("CustomScale");
        internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("CustomScaleEnabled");

        [HarmonyPatch(typeof(EditorWorldObjectCreation), "OnAddNewPrototype")]
        [HarmonyPrefix]
        private static void OnAddNewPrototype(ref Single scale)
        {
            if (Enabled)
            {
                scale = CustomScale;
            }
        }
    }
}