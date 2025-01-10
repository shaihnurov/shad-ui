using Avalonia;

namespace ShadUI.Extensions;

/// <summary>
/// Useful extensions for the <see cref="Avalonia.Controls.Button"/> class.
/// </summary>
public static class ButtonExt
{
    /// <summary>
    /// Show or hide the progress indicator.
    /// </summary>
    public static readonly AttachedProperty<bool> ShowProgressProperty =
        AvaloniaProperty.RegisterAttached<Avalonia.Controls.Button, bool>("ShowProgress", typeof(Avalonia.Controls.Button));

    /// <summary>
    /// Get the value of the <see cref="ShowProgressProperty"/>.
    /// </summary>
    /// <param name="textBox"></param>
    /// <returns></returns>
    public static bool GetShowProgress(Avalonia.Controls.Button textBox) => textBox.GetValue(ShowProgressProperty);

    /// <summary>
    /// Set the value of the <see cref="ShowProgressProperty"/>.
    /// </summary>
    /// <param name="textBox"></param>
    /// <param name="value"></param>
    public static void SetShowProgress(Avalonia.Controls.Button textBox, bool value)
    {
        textBox.SetValue(ShowProgressProperty, value);
    }
}