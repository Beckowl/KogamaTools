using System.Collections;
using System.Runtime.InteropServices;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using UnityEngine;

namespace KogamaTools.Tools.Build.UndoRedo;
internal class UndoRedoManager : MonoBehaviour
{
    private static readonly float repeatRate = GetRepeatRate();
    private static readonly float repeatDelay = GetRepeatDelay();

    private static Stack<IUndoRedoAction> undoStack = new();
    private static Stack<IUndoRedoAction> redoStack = new();

    private void Awake()
    {
        if (MVGameControllerBase.GameMode != MV.Common.MVGameMode.Edit)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (MVInputWrapper.DebugGetKeyDown(KeyCode.Z) && MVInputWrapper.DebugGetKey(KeyCode.LeftControl))
        {
            StartCoroutine(ActionCoroutine(Undo, KeyCode.Z).WrapToIl2Cpp());
        }

        if (MVInputWrapper.DebugGetKeyDown(KeyCode.Y) && MVInputWrapper.DebugGetKey(KeyCode.LeftControl))
        {
            StartCoroutine(ActionCoroutine(Redo, KeyCode.Y).WrapToIl2Cpp());
        }
    }

    internal static void PushAction(IUndoRedoAction action)
    {
#if DEBUG
        KogamaTools.mls.LogInfo($"Pushing {action.GetType()}.");
#endif

        redoStack.Clear();

        undoStack.Push(action);
    }

    internal static void Undo()
    {
        if (undoStack.Count > 0)
        {
            IUndoRedoAction action = undoStack.Pop();

            action.Undo();
            FocusOnTarget(action.Target);

            redoStack.Push(action);
        }
    }

    internal static void Redo()
    {
        if (redoStack.Count > 0)
        {
            IUndoRedoAction action = redoStack.Pop();

            action.Redo();
            FocusOnTarget(action.Target);

            undoStack.Push(action);
        }
    }

    private static void FocusOnTarget(MVWorldObjectClient target)
    {
        if (target.id == MVGameControllerBase.WOCM.RootGroup.id || target.id == -1 || target.id == 75579)
        {
            return;
        }

        Vector3 viewportPos = MVGameControllerBase.MainCameraManager.mainCamera.WorldToViewportPoint(target.transform.position);

        // focus if out of view
        if (viewportPos.x < 0 || viewportPos.x > 1 ||
            viewportPos.y < 0 || viewportPos.y > 1 ||
            viewportPos.z <= 0)
        {
            MVGameControllerBase.MainCameraManager.CurrentCamera.FocusOnObject(target);
        }
    }

    private IEnumerator ActionCoroutine(Action action, KeyCode key)
    {
        action();
        yield return new WaitForSeconds(repeatDelay);

        while (MVInputWrapper.DebugGetKey(key) && MVInputWrapper.DebugGetKey(KeyCode.LeftControl))
        {
            action();
            yield return new WaitForSeconds(repeatRate);
        }
    }

    private static float GetRepeatDelay()
    {
        if (SystemParametersInfo(0x0016, 0, out uint keyboardDelay, 0))
        {
            return (keyboardDelay + 1) * 0.25f;
        }
        return 0.5f;
    }

    private static float GetRepeatRate()
    {
        if (SystemParametersInfo(0x000A, 0, out uint keyboardSpeed, 0))
        {
            return (keyboardSpeed + 2) / 1000.0f;
        }
        return 1/31f;
    }

    [DllImport("user32.dll")]
    private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, out uint pvParam, uint fWinIni);
}
