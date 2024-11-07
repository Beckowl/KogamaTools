namespace KogamaTools.Tools.Graphics;
internal static class ThemeModifier
{

    // TODO: add options to create/modify theme previews

    internal static bool ThemesEnabled = GetCurrentTheme() != null;
    internal static Theme GetCurrentTheme()
    {
        Theme theme = MVGameControllerBase.GameMode == MV.Common.MVGameMode.CharacterEditor ? AvatarEditModeBodyController.Theme : ThemeRepository.Instance.CurrentThemeVisualization;
        return theme;
    }

    internal static void UpdateThemesEnabled()
    {
        Theme theme = GetCurrentTheme();

        if (theme != null)
        {
            if (ThemesEnabled)
            {
                theme.Activate();
            }
            else
            {
                theme.Deactivate();
            }
        }
    }

    internal static Theme CreateThemePreview(string identifier)
    {
        Theme theme = GetCurrentTheme();
        if (theme != null)
        {
            ThemeRepository.Instance.DestroyTemporary(theme);
        }
        return ThemeRepository.Instance.CreateTemporaryThemeVisualization(identifier);
    }
}
