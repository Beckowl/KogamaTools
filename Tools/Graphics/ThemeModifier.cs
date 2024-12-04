namespace KogamaTools.Tools.Graphics;
internal class ThemeModifier
{
    internal static bool ThemesEnabled = !MVGameControllerBase.SkyboxManager.enabled;
    internal static string[] ThemeIDs = { "RoundSquare", "Normal", "Christmas", "Puzzle", "Scary", "BoxPumpkin", "RoundCircleSkull", "Cartoon", "Triangles", "Pumpkin", "Animals", "Heart", "SquareSkull", "BoxHalloween", "RoundSkull", "BoxSkull", "Candy", "Square", "RoundBoxSkull", "RoundSquareSkull", "Cloudy" };
    internal static int SelectedThemePreview = 0;
    internal static Theme? Preview = null;
    internal static Theme GetCurrentTheme()
    {
        Theme theme = MVGameControllerBase.GameMode == MV.Common.MVGameMode.CharacterEditor ? AvatarEditModeBodyController.Theme : ThemeRepository.Instance.CurrentThemeVisualization;
        return theme;
    }

    internal static void ApplyToggleThemes()
    {
        ToggleAllThemes(ThemesEnabled);
    }

    internal static void CreateThemePreview()
    {
        DestroyThemePreview();

        Preview = ThemeRepository.Instance.CreateTemporaryThemeVisualization(ThemeIDs[SelectedThemePreview]);
    }

    internal static void DestroyThemePreview()
    {
        if (Preview != null)
        {
            ThemeRepository.Instance.DestroyTemporary(Preview);
            Preview = null;
        }
    }

    private static void ToggleAllThemes(bool enable)
    {
        if (!enable)
            DestroyThemePreview();


        Theme[] themes = UnityEngine.Object.FindObjectsOfType<Theme>();

        foreach (Theme theme in themes)
        {
            if (enable)
            {
                theme.Activate();
            }
            else
            {
                theme.Deactivate();
            }
        }
    }
}
