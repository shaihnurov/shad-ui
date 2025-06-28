using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
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

    /// <summary>
    ///     Defines the <see cref="ItemIconContentSpacing" /> property.
    /// </summary>
    public static readonly StyledProperty<double> ItemIconContentSpacingProperty =
        AvaloniaProperty.Register<Sidebar, double>(
            nameof(ItemIconContentSpacing));

    /// <summary>
    ///     Gets or sets the spacing between icon and content in sidebar items.
    /// </summary>
    public double ItemIconContentSpacing
    {
        get => GetValue(ItemIconContentSpacingProperty);
        set => SetValue(ItemIconContentSpacingProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="DefaultItemsSharedSizeGroup" /> property.
    /// </summary>
    public static readonly StyledProperty<string> DefaultItemsSharedSizeGroupProperty =
        AvaloniaProperty.Register<Sidebar, string>(
            nameof(DefaultItemsSharedSizeGroup));

    /// <summary>
    ///     Gets the default item SharedSizeGroup name for the sidebar.
    /// </summary>
    public string DefaultItemsSharedSizeGroup
    {
        get => GetValue(DefaultItemsSharedSizeGroupProperty);
        private set => SetValue(DefaultItemsSharedSizeGroupProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="DefaultItemsGroup" /> property.
    /// </summary>
    public static readonly StyledProperty<string> DefaultItemsGroupProperty =
        AvaloniaProperty.Register<Sidebar, string>(
            nameof(DefaultItemsGroup));

    /// <summary>
    ///     Gets the default item group name for the sidebar.
    /// </summary>
    public string DefaultItemsGroup
    {
        get => GetValue(DefaultItemsGroupProperty);
        private set => SetValue(DefaultItemsGroupProperty, value);
    }

    /// <summary>
    ///     Called when the template is applied to the control.
    /// </summary>
    /// <param name="e">The template applied event arguments.</param>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        DefaultItemsSharedSizeGroup = $"Shared{Guid.NewGuid():N}";
        DefaultItemsGroup = $"Group{Guid.NewGuid():N}";
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