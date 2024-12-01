namespace KogamaTools.Helpers
{
    internal static class RuntimeReferences
    {
        internal static DesktopEditModeController DesktopEditModeController { get; private set; } = null!;
        internal static EditorStateMachine EditorStateMachine { get; private set; } = null!;
        internal static CubeModelingStateMachine CubeModelingStateMachine { get; private set; } = null!;
        internal static EditorWorldObjectCreation EditorWorldObjectCreation { get; private set; } = null!;

        internal static void LoadReferences()
        {
            DesktopEditModeController = MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>();
            if (DesktopEditModeController == null)
            {
                throw new InvalidOperationException("Failed to initialize DesktopEditModeController.");
            }

            EditorStateMachine = DesktopEditModeController.EditModeStateMachine;
            CubeModelingStateMachine = EditorStateMachine.cubeModelingStateMachine;
            EditorWorldObjectCreation = DesktopEditModeController.editorWorldObjectCreation;
        }
    }
}
