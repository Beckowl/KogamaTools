namespace KogamaTools.Tools.Build.UndoRedo;
internal interface IUndoRedoAction
{
    MVWorldObjectClient Target { get; }
    public void Undo();
    public void Redo();
}
