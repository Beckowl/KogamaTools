using UnityEngine;

namespace KogamaTools.Tools.Build;
internal class ObjectGrouper : MonoBehaviour
{
    private static FSMEntity editModeStateMachine = null!;
    private static ESWaitForGroup grouper = new();

    internal static void GroupSelectedObjects()
    {
        if (editModeStateMachine == null)
        {
            if (MVGameControllerBase.IsInitialized && MVGameControllerBase.GameMode == MV.Common.MVGameMode.Edit)
            {
                editModeStateMachine = MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>().EditModeStateMachine;
                grouper.Enter(editModeStateMachine);
            }
            else
            {
                KogamaTools.mls.LogError("Game has not loaded yet");
            }
            return;
        }

        grouper.Enter(editModeStateMachine);
    }

    private void Update()
    {
        if (editModeStateMachine != null)
        {
            if (editModeStateMachine.lockState)
            {
                grouper.Execute(editModeStateMachine);
            }
        }
    }
}