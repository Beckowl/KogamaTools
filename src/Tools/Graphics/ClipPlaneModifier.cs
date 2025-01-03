﻿namespace KogamaTools.Tools.Graphics;
internal static class ClipPlaneModifier
{
    internal static float NearClipPlane = UnityEngine.Camera.main.nearClipPlane;
    internal static float FarClipPlane = UnityEngine.Camera.main.farClipPlane;

    internal static void ApplyChanges()
    {
        UnityEngine.Camera.main.nearClipPlane = NearClipPlane;
        UnityEngine.Camera.main.farClipPlane = FarClipPlane;
    }
}
