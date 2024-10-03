using System.Text.RegularExpressions;
using UnityEngine;

namespace KogamaTools.Helpers;
internal static class WorldObjectPicker
{
    internal static MVWorldObjectClient? Pick()
    {
        Ray ray = Camera.main.ScreenPointToRay(MVInputWrapper.GetPointerPosition());
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit))
        {
            return null;
        }

        GameObject objectHit = hit.transform.gameObject;
        Transform transformHit = objectHit.transform;

        int woid = GetWOIDFromTransform(transformHit);
        if (woid == -1)
        {
            return null;
        }

        return MVGameControllerBase.WOCM.GetWorldObjectClient(woid);
    }

    private static int GetWOIDFromTransform(Transform transform)
    {
        Match match = Regex.Match(transform.parent.name, @"id\s(\d+)");

        if (!match.Success)
        {
            return -1;
        }
        
        if (int.TryParse(match.Groups[1].Value, out int woid))
        {
            return woid;
        }

        return -1;
    }
}
