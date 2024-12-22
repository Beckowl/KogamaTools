namespace KogamaTools.Tools.Graphics;
internal enum ThemeIdentifier
{
    Animals,
    BoxHalloween,
    BoxPumpkin,
    BoxSkull,
    Candy,
    Cartoon,
    Christmas,
    Cloudy,
    Heart,
    Normal,
    Pumpkin,
    Puzzle,
    RoundBoxSkull,
    RoundCircleSkull,
    RoundSkull,
    RoundSquare,
    RoundSquareSkull,
    Scary,
    Square,
    SquareSkull,
    Triangles
}

internal static class ThemeModifier
{
    internal static bool ThemesEnabled = !MVGameControllerBase.SkyboxManager.enabled;
    internal static ThemeIdentifier SelectedTheme = GetCurrentThemeIdentifier();
    internal static Theme? Preview = null;


    internal static Theme GetCurrentTheme()
    {
        Theme theme = MVGameControllerBase.GameMode == MV.Common.MVGameMode.CharacterEditor ? AvatarEditModeBodyController.Theme : ThemeRepository.Instance.CurrentThemeVisualization;
        return theme;
    }

    internal static void ApplyToggleThemes()
    {
        ToggleAllThemes(ThemesEnabled);

        if (ThemesEnabled && GetCurrentTheme() == null && Preview == null)
        {
            CreateThemePreview();
        }
    }

    internal static void CreateThemePreview()
    {
        DestroyThemePreview();

        Preview = ThemeRepository.Instance.CreateTemporaryThemeVisualization(SelectedTheme.ToString());
    }

    internal static void DestroyThemePreview()
    {
        if (Preview != null)
        {
            ThemeRepository.Instance.DestroyTemporary(Preview);
            Preview = null;
        }
    }

    private static ThemeIdentifier GetCurrentThemeIdentifier()
    {
        if (GetCurrentTheme() != null)
        {
            return (ThemeIdentifier)Enum.Parse(typeof(ThemeIdentifier), GetCurrentTheme().Identifier);
        }

        return ThemeIdentifier.Normal;
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
