using HarmonyLib;

namespace KogamaTools.Tools.Graphics;

[HarmonyPatch]
internal static class CameraDistanceModifier
{
    internal static float distance = 10f;
    internal static void SetCameraDistance()
    {
        ThirdPersonCamera[] cameras = UnityEngine.Object.FindObjectsOfType<ThirdPersonCamera>();

        foreach (ThirdPersonCamera camera in cameras)
        {
            camera.distanceToAvatar = distance;
        }
    }
}
