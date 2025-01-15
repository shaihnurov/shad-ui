using Avalonia;

namespace ShadUI.Extensions;

/// <summary>
///     Useful extensions for the <see cref="Avalonia.Controls.Button" /> class.
/// </summary>
public static class Button
{
    /// <summary>
    ///     Show or hide the progress indicator.
    /// </summary>
    public static readonly AttachedProperty<bool> ShowProgressProperty =
        AvaloniaProperty.RegisterAttached<Avalonia.Controls.Button, bool>("ShowProgress", typeof(Avalonia.Controls.Button));

    /// <summary>
    ///     Get the value of the <see cref="ShowProgressProperty" />.
    /// </summary>
    /// <param name="textBox"></param>
    /// <returns></returns>
    public static bool GetShowProgress(Avalonia.Controls.Button textBox) => textBox.GetValue(ShowProgressProperty);

    /// <summary>
    ///     Set the value of the <see cref="ShowProgressProperty" />.
    /// </summary>
    /// <param name="textBox"></param>
    /// <param name="value"></param>
    public static void SetShowProgress(Avalonia.Controls.Button textBox, bool value)
    {
        textBox.SetValue(ShowProgressProperty, value);
    }

    /// <summary>
    ///     Add button icon.
    /// </summary>
    public static readonly AttachedProperty<object> IconProperty =
        AvaloniaProperty.RegisterAttached<Avalonia.Controls.Button, object>("Icon", typeof(Avalonia.Controls.Button));

    /// <summary>
    ///     Get the value of the <see cref="IconProperty" />.
    /// </summary>
    /// <param name="textBox"></param>
    /// <returns></returns>
    public static object GetIcon(Avalonia.Controls.Button textBox) => textBox.GetValue(IconProperty);

    /// <summary>
    ///     Set the value of the <see cref="IconProperty" />.
    /// </summary>
    /// <param name="textBox"></param>
    /// <param name="value"></param>
    public static void SetIcon(Avalonia.Controls.Button textBox, object value)
    {
        textBox.SetValue(IconProperty, value);
    }
}