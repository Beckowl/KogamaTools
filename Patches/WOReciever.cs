using HarmonyLib;

namespace KogamaTools;

[HarmonyPatch(typeof(MVWorldObjectClientManagerNetwork))]
internal static class WOReciever
{
    // a substitute for world.InitializedGameQueryData
    // can't use it because of il2cpp weirdness

    internal static OnWORecievedDelegate OnWORecieved = delegate { };
    internal delegate void OnWORecievedDelegate(MVWorldObjectClient wo);

    [HarmonyPatch("AddToWorldObjects")]
    [HarmonyPostfix]
    private static void AddToWorldObjects(MVWorldObjectClient wo)
    {
        if (MVGameControllerBase.IsInitialized)
        {
            OnWORecieved.Invoke(wo);
        }
    }
}
