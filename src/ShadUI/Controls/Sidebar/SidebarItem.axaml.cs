using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Threading;

// ReSharper disable once CheckNamespace
namespace ShadUI;

/// <summary>
///     Represents a selectable item within a sidebar control.
/// </summary>
[TemplatePart("PART_BorderContainer", typeof(Border))]
[TemplatePart("PART_ContentPresenter", typeof(ContentPresenter))]
public class SidebarItem : RadioButton
{
    /// <summary>
    ///     Defines the <see cref="IsSelected" /> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsSelectedProperty = AvaloniaProperty.Register<SidebarItem, bool>(
        nameof(IsSelected));

    /// <summary>
    ///     Gets or sets a value indicating whether the sidebar item is selected.
    /// </summary>
    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    /// <summary>
    ///     Icon property.
    /// </summary>
    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<SidebarItem, object?>(nameof(Icon));

    /// <summary>
    ///     Gets or sets the icon of the menu item.
    /// </summary>
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="Expanded" /> property.
    /// </summary>
    public static readonly StyledProperty<bool> ExpandedProperty = AvaloniaProperty.Register<SidebarItem, bool>(
        nameof(Expanded), true);

    /// <summary>
    ///     Gets or sets a value indicating whether the sidebar item is expanded.
    /// </summary>
    public bool Expanded
    {
        get => GetValue(ExpandedProperty);
        set => SetValue(ExpandedProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="Spacing" /> property.
    /// </summary>
    public static readonly StyledProperty<double> SpacingProperty = AvaloniaProperty.Register<SidebarItem, double>(
        nameof(Spacing));

    /// <summary>
    ///     Gets or sets the spacing between elements in the sidebar item.
    /// </summary>
    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="SharedSizeGroup" /> property.
    /// </summary>
    public static readonly StyledProperty<string?> SharedSizeGroupProperty =
        AvaloniaProperty.Register<SidebarItem, string?>(
            nameof(SharedSizeGroup));

    /// <summary>
    ///     Gets or sets the shared size group name for the icon column.
    /// </summary>
    public string? SharedSizeGroup
    {
        get => GetValue(SharedSizeGroupProperty);
        set => SetValue(SharedSizeGroupProperty, value);
    }

    private ColumnDefinition? _iconColumn;
    private ContentPresenter? _contentPresenter;
    private double _contentPresenterWidth;

    /// <summary>
    ///     Defines the <see cref="HasIcon" /> property.
    /// </summary>
    public static readonly StyledProperty<bool> HasIconProperty = AvaloniaProperty.Register<SidebarItem, bool>(
        nameof(HasIcon));

    /// <summary>
    ///     Gets a value indicating whether the sidebar item has an icon.
    /// </summary>
    public bool HasIcon
    {
        get => GetValue(HasIconProperty);
        private set => SetValue(HasIconProperty, value);
    }

    /// <summary>
    ///     Called when the template is applied to the control.
    /// </summary>
    /// <param name="e">The template applied event arguments.</param>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (e.NameScope.Find("PART_BorderContainer") is Border { Child: Grid { ColumnDefinitions.Count: > 0 } grid })
        {
            _iconColumn = grid.ColumnDefinitions[0];
            UpdateSharedSizeGroup();
        }

        if (e.NameScope.Find("PART_ContentPresenter") is ContentPresenter contentPresenter)
        {
            _contentPresenter = contentPresenter;
            _contentPresenterWidth = _contentPresenter.Width;

            AnimateExpand(Expanded);
        }
    }

    /// <summary>
    ///     Called when a property value changes.
    /// </summary>
    /// <param name="change">The property change event arguments.</param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SharedSizeGroupProperty)
        {
            UpdateSharedSizeGroup();
        }

        if (change.Property == ExpandedProperty)
        {
            var toExpand = change.GetNewValue<bool>();
            AnimateExpand(toExpand);
        }

        if (change.Property == IconProperty)
        {
            HasIcon = Icon != null;
        }
    }

    private void AnimateExpand(bool toExpand)
    {
        if (toExpand)
        {
            _contentPresenter?.SetValue(IsVisibleProperty, true);

            var originalTextWrapping = _contentPresenter?.GetValue(TextBlock.TextWrappingProperty);
            _contentPresenter?.SetValue(TextBlock.TextWrappingProperty, TextWrapping.NoWrap);

            _contentPresenter?.Animate(WidthProperty)
                .From(0)
                .To(_contentPresenterWidth)
                .WithDuration(TimeSpan.FromSeconds(0.1))
                .WithEasing(new EaseInOut())
                .RunAsync()
                .ContinueWith(_ => Dispatcher.UIThread.Post(() =>
                {
                    _contentPresenter.SetValue(TextBlock.TextWrappingProperty, originalTextWrapping);
                }));
            _contentPresenter?.Animate(OpacityProperty)
                .From(0)
                .To(1)
                .WithDuration(TimeSpan.FromSeconds(0.1))
                .WithEasing(new EaseInOut())
                .Start();
        }
        else
        {
            _contentPresenter?.SetValue(IsVisibleProperty, false);
        }
    }

    /// <summary>
    ///     Updates the shared size group for the icon column.
    /// </summary>
    private void UpdateSharedSizeGroup()
    {
        if (_iconColumn != null)
        {
            _iconColumn.SharedSizeGroup = SharedSizeGroup;
        }
    }
}