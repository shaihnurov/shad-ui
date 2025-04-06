using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace ShadUI.Themes;

/// <summary>
///     The main theme for the application.
/// </summary>
public class ShadTheme : Styles
{
    /// <summary>
    ///     Returns a new instance of the <see cref="ShadTheme" /> class.
    /// </summary>
    public ShadTheme()
    {
        AvaloniaXamlLoader.Load(this);
    }
}