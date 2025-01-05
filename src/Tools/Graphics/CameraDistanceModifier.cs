using HarmonyLib;
using KogamaTools.Config;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
[Section("Graphics")]
internal static class CameraDistanceModifier
{
    [Bind] internal static float distance = 10f;

    internal static void ApplyChanges()
    {
        ThirdPersonCamera[] cameras = UnityEngine.Object.FindObjectsOfType<ThirdPersonCamera>();

        foreach (ThirdPersonCamera camera in cameras)
        {
            camera.distanceToAvatar = distance;
        }
    }
}
