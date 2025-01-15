using Avalonia;
using Avalonia.Rendering.Composition;

namespace ShadUI.Extensions;

/// <summary>
///     Useful extension methods for <see cref="Avalonia.Controls.Presenters.ItemsPresenter" />.
/// </summary>
public static class ItemsPresenter
{
    /// <summary>
    ///     Gets or sets if scroll is animated.
    /// </summary>
    public static readonly AttachedProperty<bool> AnimatedScrollProperty =
        AvaloniaProperty.RegisterAttached<Avalonia.Controls.Presenters.ItemsPresenter, bool>("AnimatedScroll",
            typeof(Avalonia.Controls.Presenters.ItemsPresenter));

    static ItemsPresenter()
    {
        AnimatedScrollProperty.Changed.AddClassHandler<Avalonia.Controls.Presenters.ItemsPresenter>(HandleAnimatedScrollChanged);
    }

    private static void HandleAnimatedScrollChanged(Avalonia.Controls.Presenters.ItemsPresenter interactElem,
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
    public static bool GetAnimatedScroll(Avalonia.Controls.Presenters.ItemsPresenter wrap) => wrap.GetValue(AnimatedScrollProperty);

    /// <summary>
    ///     Sets the value of <see cref="AnimatedScrollProperty" />
    /// </summary>
    /// <param name="wrap"></param>
    /// <param name="value"></param>
    public static void SetAnimatedScroll(Avalonia.Controls.Presenters.ItemsPresenter wrap, bool value)
    {
        wrap.SetValue(AnimatedScrollProperty, value);
    }
}