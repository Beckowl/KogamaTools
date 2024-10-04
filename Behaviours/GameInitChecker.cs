
using UnityEngine;

namespace KogamaTools.Behaviours;
internal class GameInitChecker : MonoBehaviour
{
    internal static OnGameInitializedDelegate OnGameInitialized = delegate { KogamaTools.mls.LogInfo("Game is initialized."); };
    internal delegate void OnGameInitializedDelegate();

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
