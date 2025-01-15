using Avalonia;

namespace ShadUI.Extensions;

/// <summary>
///     Useful extensions for the <see cref="Avalonia.Controls.ToggleSwitch" /> class.
/// </summary>
public static class ToggleSwitch
{
    /// <summary>
    ///     Determines whether the toggle switch on/of content is right-aligned.
    /// </summary>
    public static readonly AttachedProperty<bool> RightAlignedContentProperty =
        AvaloniaProperty.RegisterAttached<Avalonia.Controls.ToggleSwitch, bool>("RightAlignedContent",
            typeof(Avalonia.Controls.ToggleSwitch));

    /// <summary>
    ///     Gets the value of the <see cref="RightAlignedContentProperty" />.
    /// </summary>
    /// <param name="toggleSwitch"></param>
    /// <returns></returns>
    public static bool GetRightAlignedContent(Avalonia.Controls.ToggleSwitch toggleSwitch) =>
        toggleSwitch.GetValue(RightAlignedContentProperty);

    /// <summary>
    ///     Sets the value of the <see cref="RightAlignedContentProperty" />.
    /// </summary>
    /// <param name="toggleSwitch"></param>
    /// <param name="value"></param>
    public static void SetRightAlignedContent(Avalonia.Controls.ToggleSwitch toggleSwitch, bool value)
    {
        toggleSwitch.SetValue(RightAlignedContentProperty, value);
    }
}