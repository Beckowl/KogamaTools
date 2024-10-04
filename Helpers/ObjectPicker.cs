using UnityEngine;

namespace KogamaTools.Helpers;
internal static class ObjectPicker
{
    internal static bool Pick(ref VoxelHit vhit, Il2CppSystem.Collections.Generic.HashSet<int> ignoreWoIds = null!, int layerMask = -262149)
    {
        Ray ray = ScreenToRay();
        float drawPlaneDist = 0f;
        bool drawPlaneHit = IsDrawPlaneHit(ref drawPlaneDist, ray);

        Il2CppSystem.Collections.Generic.List<VoxelHit> hits = CollisionDetection.MVHitAll(ray, float.PositiveInfinity, ignoreWoIds, layerMask);

        if (hits.Count == 0)
        {
            return false;
        }

        return TryGetClosestHit(ray, hits, drawPlaneHit, drawPlaneDist, ref vhit);
    }

    private static Ray ScreenToRay()
    {
        Vector3 mousePos = MVInputWrapper.GetPointerPosition();
        return EditModeObjectPicker.MainCamera.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y));
    }

    private static bool IsDrawPlaneHit(ref float drawPlaneDist, Ray ray)
    {
        if (DrawPlane.IsDrawPlaneActive)
        {
            Vector3 planeHit = Vector3.zero;
            if (DrawPlane.Pick(ref planeHit))
            {
                drawPlaneDist = (planeHit - ray.origin).magnitude;
                return true;
            }
        }
        return false;
    }

    private static bool TryGetClosestHit(Ray ray, Il2CppSystem.Collections.Generic.List<VoxelHit> hits, bool drawPlaneHit, float drawPlaneDist, ref VoxelHit vhit)
    {
        float closestDist = float.PositiveInfinity;
        bool hitDetected = false;

        foreach (VoxelHit voxelHit in hits)
        {
            if ((voxelHit.distance < drawPlaneDist || !drawPlaneHit) && voxelHit.transform.gameObject.activeInHierarchy)
            {
                float hitDist = Vector3.Distance(ray.origin, voxelHit.point);
                if (hitDist < closestDist)
                {
                    closestDist = hitDist;
                    vhit = voxelHit;
                    hitDetected = true;
                }
            }
        }
        return hitDetected;
    }
}
