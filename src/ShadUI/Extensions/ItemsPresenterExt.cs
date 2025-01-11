using Avalonia;
using Avalonia.Controls.Presenters;
using Avalonia.Rendering.Composition;

namespace ShadUI.Extensions;

/// <summary>
///     Useful extension methods for <see cref="ItemsPresenter" />.
/// </summary>
public static class ItemsPresenterExt
{
    /// <summary>
    ///     Gets or sets if scroll is animated.
    /// </summary>
    public static readonly AttachedProperty<bool> AnimatedScrollProperty =
        AvaloniaProperty.RegisterAttached<ItemsPresenter, bool>("AnimatedScroll", typeof(ItemsPresenter));

    static ItemsPresenterExt()
    {
        AnimatedScrollProperty.Changed.AddClassHandler<ItemsPresenter>(HandleAnimatedScrollChanged);
    }

    private static void HandleAnimatedScrollChanged(ItemsPresenter interactElem, AvaloniaPropertyChangedEventArgs args)
    {
        if (GetAnimatedScroll(interactElem))
            interactElem.AttachedToVisualTree += (_, _) =>
                ScrollableExt.MakeScrollable(ElementComposition.GetElementVisual(interactElem));
    }

    /// <summary>
    ///     Gets the value of <see cref="AnimatedScrollProperty" />
    /// </summary>
    /// <param name="wrap"></param>
    /// <returns></returns>
    public static bool GetAnimatedScroll(ItemsPresenter wrap) => wrap.GetValue(AnimatedScrollProperty);

    /// <summary>
    ///     Sets the value of <see cref="AnimatedScrollProperty" />
    /// </summary>
    /// <param name="wrap"></param>
    /// <param name="value"></param>
    public static void SetAnimatedScroll(ItemsPresenter wrap, bool value)
    {
        wrap.SetValue(AnimatedScrollProperty, value);
    }
}