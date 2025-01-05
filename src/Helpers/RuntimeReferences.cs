using KogamaTools.Behaviours;

namespace KogamaTools.Helpers;

internal static class RuntimeReferences
{
    internal static DesktopPlayModeController DesktopPlayModeController { get; set; } = null!;
    internal static DesktopEditModeController DesktopEditModeController { get; private set; } = null!;
    internal static EditorStateMachine EditorStateMachine { get; private set; } = null!;
    internal static CubeModelingStateMachine CubeModelingStateMachine { get; private set; } = null!;
    internal static EditorWorldObjectCreation EditorWorldObjectCreation { get; private set; } = null!;

    [InvokeOnInit(priority: 1)]
    internal static void LoadReferences()
    {
        if (MVGameControllerBase.GameMode == MV.Common.MVGameMode.Edit)
        {
            DesktopEditModeController = MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>();

            EditorStateMachine = DesktopEditModeController.EditModeStateMachine;
            CubeModelingStateMachine = EditorStateMachine.cubeModelingStateMachine;
            EditorWorldObjectCreation = DesktopEditModeController.editorWorldObjectCreation;
        }
        else if (MVGameControllerBase.GameMode == MV.Common.MVGameMode.Play)
        {
            DesktopPlayModeController = MVGameControllerBase.PlayModeUI.Cast<DesktopPlayModeController>();
        }
    }
}
