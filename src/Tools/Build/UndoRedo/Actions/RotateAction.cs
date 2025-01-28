using HarmonyLib;
using UGUI.Desktop.Scripts.EditMode.Gizmo;
using UnityEngine;

namespace KogamaTools.Tools.Build.UndoRedo.Actions;

[HarmonyPatch]
internal class RotateAction : IUndoRedoAction
{
    public MVWorldObjectClient Target => target;

    private MVWorldObjectClient target;
    private Vector3 oldPos;
    private Vector3 newPos;
    private Quaternion oldRotation;
    private Quaternion newRotation;

    public RotateAction(MVWorldObjectClient target, Vector3 oldPos, Vector3 newPos, Quaternion oldRotation, Quaternion newRotation)
    {
        this.target = target;
        this.oldPos = oldPos;
        this.newPos = newPos;
        this.oldRotation = oldRotation;
        this.newRotation = newRotation;
    }

    public void Redo()
    {
        target.transform.position = newPos;
        target.transform.rotation = newRotation;
    }

    public void Undo()
    {
        target.transform.position = oldPos;
        target.transform.rotation = oldRotation;
    }

    [HarmonyPatch(typeof(RotationHelper), "RotateStep")]
    [HarmonyPatch(typeof(RotationHelper), "ResetRotation")]
    [HarmonyPrefix]
    private static void ApplyRotation(RotationHelper __instance, ref (MVWorldObjectClient target, Vector3 oldPos, Quaternion oldRotation) __state)
    {
        var e = __instance.editorStateMachine;

        if (e.SingleSelectedWO != null)
        {
            __state.target = e.SingleSelectedWO;
            __state.oldPos = e.SingleSelectedWO.transform.position;
            __state.oldRotation = e.SingleSelectedWO.transform.rotation;
        }
    }

    [HarmonyPatch(typeof(RotationHelper), "RotateStep")]
    [HarmonyPatch(typeof(RotationHelper), "ResetRotation")]
    [HarmonyPostfix]
    private static void RotateStepPostfix((MVWorldObjectClient target, Vector3 oldPos, Quaternion oldRotation) __state)
    {
        UndoRedoManager.PushAction(new RotateAction(__state.target, __state.oldPos, __state.target.transform.position, __state.oldRotation, __state.target.transform.rotation));
    }
}
