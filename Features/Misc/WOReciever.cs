using HarmonyLib;

namespace KogamaTools.Features.Misc;

[HarmonyPatch]
internal static class WOReciever
{
    // a substitute for world.InitializedGameQueryData
    // can't use it because of il2cpp weirdness

    internal static OnWORecievedDelegate OnWORecieved = delegate { };
    internal delegate void OnWORecievedDelegate(MVWorldObjectClient wo, int instigatorActorNumber);

    [HarmonyPatch(typeof(WorldNetwork), "CreateQueryEvent")]
    [HarmonyPostfix]
    private static void AddToWorldObjects(MVWorldObjectClient root, int instigatorActorNumber)
    {
        if (MVGameControllerBase.IsInitialized)
        {
            OnWORecieved.Invoke(root, instigatorActorNumber);
        }
    }
}
