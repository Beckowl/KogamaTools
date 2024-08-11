using HarmonyLib;
using KogamaTools.Helpers;

namespace KogamaTools.Patches
{
    [HarmonyPatch(typeof(EditorWorldObjectCreation))]
    internal static class CustomModelScale
    {
        internal static Single CustomScale = ConfigHelper.GetConfigValue<Single>("CustomScale");
        internal static bool Enabled = ConfigHelper.GetConfigValue<bool>("CustomScaleEnabled");

        [HarmonyPatch("OnAddNewPrototype")]
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