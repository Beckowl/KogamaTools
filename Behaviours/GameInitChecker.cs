
using KogamaTools.Tools.Misc;
using UnityEngine;

namespace KogamaTools.Behaviours;
internal class GameInitChecker : MonoBehaviour
{
    internal static OnGameInitializedDelegate OnGameInitialized = delegate { KogamaTools.mls.LogInfo("Game is initialized."); };
    internal delegate void OnGameInitializedDelegate();

    static GameInitChecker()
    {
        OnGameInitialized += GreetingMessage.JoinNotification;
    }

    private static bool initialized = false;
    void Update()
    {
        if (MVGameControllerBase.IsInitialized && !initialized)
        {
            OnGameInitialized.Invoke();
            initialized = true;
        }
    }


}