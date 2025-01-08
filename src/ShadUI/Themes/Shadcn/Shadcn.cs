using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Styling;
using ShadUI.Controls;

namespace ShadUI.Themes.Shadcn;

public static class Shadcn
{
    public static void Configure(ThemeVariant startupTheme)
    {
        var application = Application.Current;

        if (application == null) return;

        application.Resources.MergedDictionaries.Add(
            new ResourceInclude(new Uri("avares://ShadUI/Themes/Shadcn/BlackWhiteTheme.axaml"))
            {
                Source = new Uri("avares://ShadUI/Themes/Shadcn/BlackWhiteTheme.axaml")
            });

        var whiteTheme = new ColorTheme("White", new Color(255, 255, 255, 255), new Color(255, 255, 255, 255));
        var blackTheme = new ColorTheme("Black", new Color(255, 9, 9, 11), new Color(255, 9, 9, 11));

        BaseTheme.GetInstance().AddColorThemes([whiteTheme, blackTheme]);

        if (application.ApplicationLifetime is not ClassicDesktopStyleApplicationLifetime desktop) return;

        if (desktop.MainWindow is not Window window) return;
        var blackStyles = new StyleInclude(new Uri("avares://ShadUI/Themes/Shadcn/ShadDarkStyles.axaml"))
        {
            Source = new Uri("avares://ShadUI/Themes/Shadcn/ShadDarkStyles.axaml")
        };

        var lightStyles = new StyleInclude(new Uri("avares://ShadUI/Themes/Shadcn/ShadLightStyles.axaml"))
        {
            Source = new Uri("avares://ShadUI/Themes/Shadcn/ShadLightStyles.axaml")
        };

        window.BackgroundShaderFile = startupTheme == ThemeVariant.Dark ? "background-dark" : "background";
        ToggleStyle(startupTheme);

        BaseTheme.GetInstance().OnBaseThemeChanged += ToggleStyle;
        return;

        void ToggleStyle(ThemeVariant variant)
        {
            if (variant == ThemeVariant.Dark)
            {
                application.Styles.Add(blackStyles);
                application.Styles.Remove(lightStyles);
                BaseTheme.GetInstance().ChangeColorTheme(whiteTheme);
                window.BackgroundShaderFile = "background-dark";
            }
            else
            {
                BaseTheme.GetInstance().ChangeColorTheme(blackTheme);
                application.Styles.Remove(blackStyles);
                application.Styles.Add(lightStyles);
                window.BackgroundShaderFile = "background";
            }
        }
    }
}