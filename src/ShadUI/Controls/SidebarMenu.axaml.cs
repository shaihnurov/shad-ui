using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace ShadUI.Controls;

/// <summary>
///     A control that provides a sidebar menu.
/// </summary>
public class SidebarMenu : SelectingItemsControl
{
    /// <summary>
    ///     Enables or disables the sidebar toggle button.
    /// </summary>
    public static readonly StyledProperty<bool> SidebarToggleEnabledProperty =
        AvaloniaProperty.Register<Window, bool>(nameof(SidebarToggleEnabled), true);

    /// <summary>
    ///     Gets or sets a value indicating whether the sidebar toggle button is enabled.
    /// </summary>
    public bool SidebarToggleEnabled
    {
        get => GetValue(SidebarToggleEnabledProperty);
        set => SetValue(SidebarToggleEnabledProperty, value);
    }

    /// <summary>
    ///     Indicates whether the toggle button is visible.
    /// </summary>
    public static readonly StyledProperty<bool> IsToggleButtonVisibleProperty =
        AvaloniaProperty.Register<SidebarMenu, bool>(nameof(IsToggleButtonVisible), true);

    /// <summary>
    ///     Gets or sets a value indicating whether the toggle button is visible.
    /// </summary>
    public bool IsToggleButtonVisible
    {
        get => GetValue(IsToggleButtonVisibleProperty);
        set => SetValue(IsToggleButtonVisibleProperty, value);
    }

    /// <summary>
    ///     Indicates whether the menu is expanded.
    /// </summary>
    public static readonly StyledProperty<bool> IsMenuExpandedProperty =
        AvaloniaProperty.Register<SidebarMenu, bool>(nameof(IsMenuExpanded), true);

    /// <summary>
    ///     Gets or sets a value indicating whether the menu is expanded.
    /// </summary>
    public bool IsMenuExpanded
    {
        get => GetValue(IsMenuExpandedProperty);
        set => SetValue(IsMenuExpandedProperty, value);
    }

    /// <summary>
    ///     Length of the open pane.
    /// </summary>
    public static readonly StyledProperty<int> OpenPaneLengthProperty =
        AvaloniaProperty.Register<SidebarMenu, int>(nameof(OpenPaneLength), 220);

    /// <summary>
    ///     Gets or sets the length of the open pane.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public int OpenPaneLength
    {
        get => GetValue(OpenPaneLengthProperty);
        set => SetValue(OpenPaneLengthProperty, value switch
        {
            >= 200 => value,
            _ => throw new ArgumentOutOfRangeException($"OpenPaneLength must be greater than or equal to 200, but was {value}")
        });
    }

    /// <summary>
    ///     Header minimum height.
    /// </summary>
    public static readonly StyledProperty<double> HeaderMinHeightProperty =
        AvaloniaProperty.Register<SidebarMenu, double>(nameof(HeaderMinHeight));

    /// <summary>
    ///     Gets or sets the header minimum height.
    /// </summary>
    public double HeaderMinHeight
    {
        get => GetValue(HeaderMinHeightProperty);
        set => SetValue(HeaderMinHeightProperty, value);
    }

    /// <summary>
    ///     Header content.
    /// </summary>
    public static readonly StyledProperty<object?> HeaderContentProperty =
        AvaloniaProperty.Register<SidebarMenu, object?>(nameof(HeaderContent));

    /// <summary>
    ///     Gets or sets the header content.
    /// </summary>
    public object? HeaderContent
    {
        get => GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    /// <summary>
    ///     Body content.
    /// </summary>
    public static readonly StyledProperty<object?> BodyContentProperty =
        AvaloniaProperty.Register<SidebarMenu, object?>(nameof(BodyContent));

    /// <summary>
    ///     Gets or sets the body content.
    /// </summary>
    public object? BodyContent
    {
        get => GetValue(BodyContentProperty);
        set => SetValue(BodyContentProperty, value);
    }
    
    /// <summary>
    ///     Footer content.
    /// </summary>
    public static readonly StyledProperty<object?> FooterContentProperty =
        AvaloniaProperty.Register<SidebarMenu, object?>(nameof(FooterContent));

    /// <summary>
    ///     Gets or sets the footer content.
    /// </summary>
    public object? FooterContent
    {
        get => GetValue(FooterContentProperty);
        set => SetValue(FooterContentProperty, value);
    }

    private bool IsSpacerVisible => !IsMenuExpanded;

    private Grid? _spacer;

    /// <summary>
    ///     Returns a new instance of the <see cref="SidebarMenu" /> class.
    /// </summary>
    public SidebarMenu()
    {
        SelectionMode = SelectionMode.Single | SelectionMode.AlwaysSelected;
    }

    private void MenuExpandedClicked()
    {
        IsMenuExpanded = !IsMenuExpanded;

        UpdateMenuItemsExpansion();
    }

    private void UpdateMenuItemsExpansion()
    {
        if (_sideMenuItems.Any())
            foreach (var item in _sideMenuItems)
                item.IsTopMenuExpanded = IsMenuExpanded;

        else if (Items.FirstOrDefault() is SidebarMenuItem)
            foreach (SidebarMenuItem? item in Items)
                item!.IsTopMenuExpanded = IsMenuExpanded;
    }

    /// <summary>
    ///     Called when the control is added to a visual tree.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        e.NameScope.Get<Button>("PART_SidebarToggleButton").Click += (_, _) => MenuExpandedClicked();
        _spacer = e.NameScope.Get<Grid>("PART_Spacer");
        if (_spacer != null) _spacer.IsVisible = IsSpacerVisible;
    }

    /// <summary>
    ///     Called when the control is loaded.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        UpdateMenuItemsExpansion();
    }

    /// <summary>
    ///     Called when a property changes.
    /// </summary>
    /// <param name="change"></param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsMenuExpandedProperty && _spacer != null)
            _spacer.IsVisible = IsSpacerVisible;
    }

    /// <summary>
    /// Updates the selection from the pointer event source.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public bool UpdateSelectionFromPointerEvent(Control source) => UpdateSelectionFromEventSource(source);

    /// <summary>
    /// Creates a new container for the item.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index"></param>
    /// <param name="recycleKey"></param>
    /// <returns></returns>
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        var menuItem = ItemTemplate != null && ItemTemplate.Match(item) &&
                       ItemTemplate.Build(item) is SidebarMenuItem menu
            ? menu
            : new SidebarMenuItem();
        _sideMenuItems.Add(menuItem);
        return menuItem;
    }

    private readonly List<SidebarMenuItem> _sideMenuItems = [];

    /// <summary>
    /// Returns if the container needs.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index"></param>
    /// <param name="recycleKey"></param>
    /// <returns></returns>
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey) =>
        NeedsContainer<SidebarMenuItem>(item, out recycleKey);
}