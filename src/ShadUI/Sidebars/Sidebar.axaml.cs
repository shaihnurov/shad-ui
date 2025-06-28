using System;
using Avalonia;
using Avalonia.Controls;
using ShadUI.Utilities;

// ReSharper disable once CheckNamespace
namespace ShadUI;

/// <summary>
///     Represents a sidebar control that can be expanded or collapsed.
/// </summary>
public class Sidebar : ContentControl
{
    /// <summary>
    ///     Defines the <see cref="Expanded" /> property.
    /// </summary>
    public static readonly StyledProperty<bool> ExpandedProperty = AvaloniaProperty.Register<Sidebar, bool>(
        nameof(Expanded), true);

    /// <summary>
    ///     Gets or sets a value indicating whether the sidebar is expanded.
    /// </summary>
    public bool Expanded
    {
        get => GetValue(ExpandedProperty);
        set => SetValue(ExpandedProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="Header" /> property.
    /// </summary>
    public static readonly StyledProperty<object?> HeaderProperty = AvaloniaProperty.Register<Sidebar, object?>(
        nameof(Header));

    /// <summary>
    ///     Gets or sets the header content of the sidebar.
    /// </summary>
    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="Footer" /> property.
    /// </summary>
    public static readonly StyledProperty<object?> FooterProperty = AvaloniaProperty.Register<Sidebar, object?>(
        nameof(Footer));

    /// <summary>
    ///     Gets or sets the footer content of the sidebar.
    /// </summary>
    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    private double _width;

    /// <summary>
    ///     Called when a property value changes.
    /// </summary>
    /// <param name="change">The property change event arguments containing information about the changed property.</param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ExpandedProperty)
        {
            var toExpand = change.GetNewValue<bool>();

            AnimateOnExpand(toExpand);
        }
    }

    private void AnimateOnExpand(bool toExpand)
    {
        if (!toExpand) _width = Width;

        if (toExpand)
        {
            this.Animate(WidthProperty)
                .From(MinWidth)
                .To(_width)
                .WithEasing(new EaseInOutBack { BounceIntensity = EasingIntensity.Strong })
                .WithDuration(TimeSpan.FromMilliseconds(300))
                .Start();

            if (MinWidth == 0)
            {
                this.Animate(OpacityProperty)
                    .From(0.0)
                    .To(1.0)
                    .WithEasing(new EaseInOut())
                    .WithDuration(TimeSpan.FromMilliseconds(300))
                    .Start();
            }
        }
        else
        {
            this.Animate(WidthProperty)
                .From(_width)
                .To(MinWidth)
                .WithEasing(new EaseOut())
                .WithDuration(TimeSpan.FromMilliseconds(300))
                .Start();

            if (MinWidth == 0)
            {
                this.Animate(OpacityProperty)
                    .From(1.0)
                    .To(0.0)
                    .WithEasing(new EaseOut())
                    .WithDuration(TimeSpan.FromMilliseconds(300))
                    .Start();
            }
        }
    }
}