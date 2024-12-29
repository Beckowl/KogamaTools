using HarmonyLib;

namespace KogamaTools.Tools.Misc;

[HarmonyPatch]
internal static class WOReciever
{
    // a substitute for world.InitializedGameQueryData
    // can't use it because of il2cpp weirdness

    internal static OnWORecievedDelegate OnWORecieved = delegate { };
    internal delegate void OnWORecievedDelegate(MVWorldObjectClient wo, int instigatorActorNumber);

    [HarmonyPatch(typeof(WorldNetwork), "CreateQueryEvent")]
    [HarmonyPostfix]
    private static void CreateQueryEvent(MVWorldObjectClient root, int instigatorActorNumber)
    {
        if (MVGameControllerBase.IsInitialized)
        {
#if DEBUG
            KogamaTools.mls.LogInfo($"CreateQueryEvent: {root.ToString()},\t{instigatorActorNumber}");
#endif
            OnWORecieved.Invoke(root, instigatorActorNumber);
        }
    }
}
