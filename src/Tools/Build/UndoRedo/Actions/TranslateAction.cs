using HarmonyLib;
using UnityEngine;

namespace KogamaTools.Tools.Build.UndoRedo.Actions;

[HarmonyPatch]
internal class TranslateAction : IUndoRedoAction
{
    public MVWorldObjectClient Target => target;

    private MVWorldObjectClient target;
    private Vector3 oldPos;
    private Vector3 newPos;

    private static MVWorldObjectClient targetRecord;
    private static Vector3 oldPosRecord;

    public TranslateAction(MVWorldObjectClient target, Vector3 oldPos, Vector3 newPos)
    {
        this.target = target;
        this.oldPos = oldPos;
        this.newPos = newPos;
    }

    public void Redo()
    {
        target.transform.position = newPos;
    }

    public void Undo()
    {
        target.transform.position = oldPos;
    }

    [HarmonyPatch(typeof(ESTranslate), "Enter")]
    [HarmonyPrefix]
    private static void Enter(EditorStateMachine e)
    {
        if (e.SingleSelectedWO != null)
        {
            targetRecord = e.SingleSelectedWO;

            oldPosRecord = e.SingleSelectedWO.transform.position;
        }
    }

    [HarmonyPatch(typeof(ESTranslate), "Exit")]
    [HarmonyPrefix]
    private static void Exit(EditorStateMachine e)
    {
        if (e.SingleSelectedWO != null)
        {
            UndoRedoManager.PushAction(new TranslateAction(targetRecord, oldPosRecord, e.SingleSelectedWO.transform.position));
        }
    }
}