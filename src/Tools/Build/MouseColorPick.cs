using KogamaTools.Behaviours;
using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Tools.Build;
internal static class MouseColorPick
{

    [InvokeOnInit]
    internal static void SubscribeHotkeys()
    {
        HotkeySubscriber.Subscribe(KeyCode.Mouse2, DoPicking);
    }

    private static void DoPicking()
    {
        if (MVGameControllerBase.Game.IsPlaying) return;

        CubeModelingStateMachine e = RuntimeReferences.CubeModelingStateMachine;
        CubePickingInfo pickingInfo = new();

        if (EditModeObjectPicker.GetPickingInfo(e.TargetCubeModel, ref pickingInfo))
        {
            byte pickedMaterial = pickingInfo.cube.faceMaterials[(int)pickingInfo.pickedFace];

#if DEBUG
            KogamaTools.mls.LogInfo($"Mouse-picked {pickedMaterial}");
#endif
            if (MVMaterialRepository.instance.IsMaterialUnlocked(pickedMaterial))
            {
                e.CurrentMaterialId = pickedMaterial;
            }
            else
            {
                ModelCursor.ShowUnlockMaterialNotification();
            }
        }
    }
}
