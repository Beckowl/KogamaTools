using UnityEngine;
namespace KogamaTools.Behaviours;
internal class ObjectGrouper : MonoBehaviour
{
    private static DesktopEditModeController? controller;
    private static ESWaitForGroup grouper = new();

    internal static void GroupSelectedObjects()
    {
        if (controller != null)
        {
            grouper.Enter(controller.EditModeStateMachine);
        }
        else
        {
            KogamaTools.mls.LogError("Game is not loaded yet");
        }
    }

    private void Update()
    {
        if (MVGameControllerBase.IsInitialized)
        {
            if (controller == null)
            {
                controller = MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>();
            }
            if (controller.EditModeStateMachine.lockState)
            {
                grouper.Execute(controller.EditModeStateMachine);
            }
        }
    }
}