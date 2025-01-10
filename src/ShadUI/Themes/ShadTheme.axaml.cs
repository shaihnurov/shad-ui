using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace ShadUI.Themes;

/// <summary>
///    The main theme for the application.
/// </summary>
public class ShadTheme : Styles
{
    /// <summary>
    ///     Called whenever the application's <see cref="ThemeVariant" /> is changed.
    ///     Useful where controls need to change based on light/dark.
    /// </summary>
    public Action<ThemeVariant>? OnBaseThemeChanged { get; set; }

    private readonly Application _app;

    /// <summary>
    /// Returns a new instance of the <see cref="ShadTheme"/> class.
    /// </summary>
    public ShadTheme()
    {
        AvaloniaXamlLoader.Load(this);
        _app = Application.Current!;
        _app.ActualThemeVariantChanged += (_, _) => OnBaseThemeChanged?.Invoke(_app.ActualThemeVariant);
    }

    /// <summary>
    ///     Retrieves an instance tied to the currently active application.
    /// </summary>
    /// <returns>A <see cref="ShadTheme" /> instance that can be used to change themes.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no Theme has been defined in App.axaml.</exception>
    public static ShadTheme GetInstance() => [];
}