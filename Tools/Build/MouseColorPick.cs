using KogamaTools.Behaviours;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Tools.Build;
internal static class MouseColorPick
{
    internal static void SubscribeHotkeys()
    {
        HotkeySubscriber.Subscribe(KeyCode.Mouse2, DoPicking);
    }

    private static void DoPicking()
    {
        CubeModelingStateMachine e = RuntimeReferences.CubeModelingStateMachine;
        CubePickingInfo pickingInfo = new();

        if (EditModeObjectPicker.GetPickingInfo(e.TargetCubeModel, ref pickingInfo))
        {
            e.CurrentMaterialId = pickingInfo.cube.faceMaterials[(int)pickingInfo.pickedFace];
        }
    }
}
