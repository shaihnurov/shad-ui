using Avalonia;
using Avalonia.Rendering.Composition;

namespace ShadUI.Extensions;

/// <summary>
///     Useful extension methods for <see cref="Avalonia.Controls.StackPanel" />
/// </summary>
public static class StackPanel
{
    /// <summary>
    ///     Gets or sets if scroll is animated.
    /// </summary>
    public static readonly AttachedProperty<bool> AnimatedScrollProperty =
        AvaloniaProperty.RegisterAttached<Avalonia.Controls.StackPanel, bool>("AnimatedScroll",
            typeof(Avalonia.Controls.StackPanel));

    static StackPanel()
    {
        AnimatedScrollProperty.Changed.AddClassHandler<Avalonia.Controls.StackPanel>(HandleAnimatedScrollChanged);
    }

    private static void HandleAnimatedScrollChanged(Avalonia.Controls.StackPanel interactElem,
        AvaloniaPropertyChangedEventArgs args)
    {
        if (GetAnimatedScroll(interactElem))
            interactElem.AttachedToVisualTree += (_, _) =>
                Scrollable.MakeScrollable(ElementComposition.GetElementVisual(interactElem));
    }

    /// <summary>
    ///     Gets the value of <see cref="AnimatedScrollProperty" />
    /// </summary>
    /// <param name="wrap"></param>
    /// <returns></returns>
    public static bool GetAnimatedScroll(Avalonia.Controls.StackPanel wrap) => wrap.GetValue(AnimatedScrollProperty);

    /// <summary>
    ///     Sets the value of <see cref="AnimatedScrollProperty" />
    /// </summary>
    /// <param name="wrap"></param>
    /// <param name="value"></param>
    public static void SetAnimatedScroll(Avalonia.Controls.StackPanel wrap, bool value)
    {
        wrap.SetValue(AnimatedScrollProperty, value);
    }
}