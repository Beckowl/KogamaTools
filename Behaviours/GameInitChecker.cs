using UnityEngine;

namespace KogamaTools.Behaviours;
internal class GameInitChecker : MonoBehaviour
{
    internal static OnGameInitializedDelegate OnGameInitialized = delegate { };
    internal delegate void OnGameInitializedDelegate();

    internal static bool IsInitialized = false;

    void Update()
    {
        if (MVGameControllerBase.IsInitialized && !IsInitialized)
        {
            OnGameInitialized.Invoke();
            IsInitialized = true;
            Destroy(this);
        }
    }
}
