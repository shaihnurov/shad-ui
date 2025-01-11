using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace ShadUI.Controls;

/// <summary>
///     Represents a menu item in a <see cref="SidebarMenu" />.
/// </summary>
public class SidebarMenuItem : ListBoxItem
{
    /// <summary>
    ///     Icon property.
    /// </summary>
    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<SidebarMenuItem, object?>(nameof(Icon));

    private static readonly Point InvalidPoint = new(double.NaN, double.NaN);
    private Point _pointerDownPoint = InvalidPoint;

    /// <summary>
    ///     Gets or sets the icon of the menu item.
    /// </summary>
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    ///     Header property.
    /// </summary>
    public static readonly StyledProperty<string?> HeaderProperty =
        AvaloniaProperty.Register<SidebarMenuItem, string?>(nameof(Header));

    /// <summary>
    ///     Gets or sets the header of the menu item.
    /// </summary>
    public string? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    ///     Called when the control template is applied.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (e.NameScope.Get<ContentPresenter>("PART_AltDisplay") is { } contentControl)
            if (Header is not null || Icon is not null)
                contentControl.IsVisible = false;
    }

    /// <summary>
    ///     Called when a pointer is pressed.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        _pointerDownPoint = InvalidPoint;

        if (e.Handled)
            return;

        if (!e.Handled && ItemsControl.ItemsControlFromItemContainer(this) is SidebarMenu owner)
        {
            var p = e.GetCurrentPoint(this);

            if (p.Properties.PointerUpdateKind is PointerUpdateKind.LeftButtonPressed or
                PointerUpdateKind.RightButtonPressed)
            {
                if (p.Pointer.Type == PointerType.Mouse)
                    // If the pressed point comes from a mouse, perform the selection immediately.
                    e.Handled = owner.UpdateSelectionFromPointerEvent(this);
                else
                    // Otherwise perform the selection when the pointer is released as to not
                    // interfere with gestures.
                    _pointerDownPoint = p.Position;
                // Ideally we'd set handled here, but that would prevent the scroll gesture
                // recognizer from working.
                ////e.Handled = true;
            }
        }
    }

    /// <summary>
    ///     Called when a pointer is released.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (!e.Handled &&
            !double.IsNaN(_pointerDownPoint.X) &&
            e.InitialPressMouseButton is MouseButton.Left or MouseButton.Right)
        {
            var point = e.GetCurrentPoint(this);
            var settings = TopLevel.GetTopLevel(e.Source as Visual)?.PlatformSettings;
            var tapSize = settings?.GetTapSize(point.Pointer.Type) ?? new Size(4, 4);
            var tapRect = new Rect(_pointerDownPoint, new Size())
                .Inflate(new Thickness(tapSize.Width, tapSize.Height));

            if (new Rect(Bounds.Size).ContainsExclusive(point.Position) &&
                tapRect.ContainsExclusive(point.Position) &&
                ItemsControl.ItemsControlFromItemContainer(this) is SidebarMenu owner)
                if (owner.UpdateSelectionFromPointerEvent(this))
                {
                    // As we only update selection from touch/pen on pointer release, we need to raise
                    // the pointer event on the owner to trigger a commit.
                    if (e.Pointer.Type != PointerType.Mouse)
                    {
                        var sourceBackup = e.Source;
                        owner.RaiseEvent(e);
                        e.Source = sourceBackup;
                    }

                    e.Handled = true;
                }
        }

        _pointerDownPoint = InvalidPoint;
    }

    /// <summary>
    ///     Animate the content of the menu item.
    /// </summary>
    public static readonly StyledProperty<bool> AnimateContentProperty =
        AvaloniaProperty.Register<SidebarMenuItem, bool>(nameof(AnimateContent), true);

    /// <summary>
    ///     Gets or sets if the content of the menu item is movable.
    /// </summary>
    public bool AnimateContent
    {
        get => GetValue(AnimateContentProperty);
        set => SetValue(AnimateContentProperty, value);
    }

    /// <summary>
    ///     Returns if the top menu is expanded.
    /// </summary>
    public static readonly StyledProperty<bool> IsTopMenuExpandedProperty =
        AvaloniaProperty.Register<SidebarMenuItem, bool>(nameof(IsTopMenuExpanded), true);

    /// <summary>
    ///     Gets or sets if the top menu is expanded.
    /// </summary>
    public bool IsTopMenuExpanded
    {
        get => GetValue(IsTopMenuExpandedProperty);
        set => SetValue(IsTopMenuExpandedProperty, value);
    }
}