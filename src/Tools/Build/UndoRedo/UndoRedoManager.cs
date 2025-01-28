using KogamaTools.Behaviours;
using UnityEngine;

namespace KogamaTools.Tools.Build.UndoRedo;
internal static class UndoRedoManager
{
    private static Stack<IUndoRedoAction> undoStack = new();
    private static Stack<IUndoRedoAction> redoStack = new();

    [InvokeOnInit]
    internal static void RegisterHotkeys()
    {
        HotkeySubscriber.Subscribe(KeyCode.Z, () =>
        {
            if (MVInputWrapper.DebugGetKey(KeyCode.LeftControl))
                Undo();
        });

        HotkeySubscriber.Subscribe(KeyCode.Y, () =>
        {
            if (MVInputWrapper.DebugGetKey(KeyCode.LeftControl))
                Redo();
        });
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
#if DEBUG
            KogamaTools.mls.LogInfo($"undoing {action.GetType()}.");
#endif

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
#if DEBUG
            KogamaTools.mls.LogInfo($"redoing {action.GetType()}.");
#endif

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
}
