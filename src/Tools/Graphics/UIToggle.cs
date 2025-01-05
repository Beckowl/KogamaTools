using KogamaTools.Behaviours;
using KogamaTools.Config;
using KogamaTools.Helpers;

namespace KogamaTools.Tools.Graphics;

[Section("Graphics")]
internal static class UIToggle
{
    [Bind] internal static bool UIVisible = true;

    [InvokeOnInit]
    internal static void UpdateUIVisibility()
    {
        switch (MVGameControllerBase.GameMode)
        {
            case MV.Common.MVGameMode.Edit:
                if (RuntimeReferences.DesktopEditModeController.IsInPlayInEditMode)
                {
                    RuntimeReferences.DesktopPlayModeController.gameObject.SetActive(UIVisible);
                }
                else
                {
                    RuntimeReferences.DesktopEditModeController.gameObject.SetActive(UIVisible);
                }
                break;
            case MV.Common.MVGameMode.Play:
                RuntimeReferences.DesktopPlayModeController.gameObject.SetActive(UIVisible);
                break;
            case MV.Common.MVGameMode.CharacterEditor:
                MVGameControllerDesktop.Instance.modeController.gameObject.SetActive(UIVisible);
                break;
        }
    }
}
