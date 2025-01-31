using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using ShadUI.Extensions;
using Button = Avalonia.Controls.Button;

namespace ShadUI.Controls;

/// <summary>
///     A modern window with a customizable title bar.
/// </summary>
public class Window : Avalonia.Controls.Window
{
    /// <summary>
    ///     The style key of the window.
    /// </summary>
    protected override Type StyleKeyOverride => typeof(Window);

    /// <summary>
    ///     The font size of the title.
    /// </summary>
    public static readonly StyledProperty<double> TitleFontSizeProperty =
        AvaloniaProperty.Register<Window, double>(nameof(TitleFontSize), 13);

    /// <summary>
    ///     Gets or sets the value of the <see cref="TitleFontSizeProperty" />.
    /// </summary>
    public double TitleFontSize
    {
        get => GetValue(TitleFontSizeProperty);
        set => SetValue(TitleFontSizeProperty, value);
    }

    /// <summary>
    ///     The font weight of the title.
    /// </summary>
    public static readonly StyledProperty<FontWeight> TitleFontWeightProperty =
        AvaloniaProperty.Register<Window, FontWeight>(nameof(TitleFontWeight), FontWeight.Bold);

    /// <summary>
    ///     Gets or sets the value of the <see cref="TitleFontWeightProperty" />.
    /// </summary>
    public FontWeight TitleFontWeight
    {
        get => GetValue(TitleFontWeightProperty);
        set => SetValue(TitleFontWeightProperty, value);
    }

    /// <summary>
    ///     The content of the logo.
    /// </summary>
    public static readonly StyledProperty<Control?> LogoContentProperty =
        AvaloniaProperty.Register<Window, Control?>(nameof(LogoContent));

    /// <summary>
    ///     Gets or sets the value of the <see cref="LogoContentProperty" />.
    /// </summary>
    public Control? LogoContent
    {
        get => GetValue(LogoContentProperty);
        set => SetValue(LogoContentProperty, value);
    }

    /// <summary>
    ///     Whether to show the bottom border.
    /// </summary>
    public static readonly StyledProperty<bool> ShowBottomBorderProperty =
        AvaloniaProperty.Register<Window, bool>(nameof(ShowBottomBorder), true);

    /// <summary>
    ///     Gets or sets the value of the <see cref="ShowBottomBorderProperty" />.
    /// </summary>
    public bool ShowBottomBorder
    {
        get => GetValue(ShowBottomBorderProperty);
        set => SetValue(ShowBottomBorderProperty, value);
    }

    /// <summary>
    ///     Whether to show the title bar.
    /// </summary>
    public static readonly StyledProperty<bool> IsTitleBarVisibleProperty =
        AvaloniaProperty.Register<Window, bool>(nameof(IsTitleBarVisible), true);

    /// <summary>
    ///     Gets or sets the value of the <see cref="IsTitleBarVisibleProperty" />.
    /// </summary>
    public bool IsTitleBarVisible
    {
        get => GetValue(IsTitleBarVisibleProperty);
        set => SetValue(IsTitleBarVisibleProperty, value);
    }

    /// <summary>
    ///     Whether to enable title bar animation.
    /// </summary>
    public static readonly StyledProperty<bool> TitleBarAnimationEnabledProperty =
        AvaloniaProperty.Register<Window, bool>(nameof(TitleBarAnimationEnabled), true);

    /// <summary>
    ///     Gets or sets the value of the <see cref="TitleBarAnimationEnabledProperty" />.
    /// </summary>
    public bool TitleBarAnimationEnabled
    {
        get => GetValue(TitleBarAnimationEnabledProperty);
        set => SetValue(TitleBarAnimationEnabledProperty, value);
    }

    /// <summary>
    ///     Whether to show the menu.
    /// </summary>
    public static readonly StyledProperty<bool> IsMenuVisibleProperty =
        AvaloniaProperty.Register<Window, bool>(nameof(IsMenuVisible));

    /// <summary>
    ///     Gets or sets the value of the <see cref="IsMenuVisibleProperty" />.
    /// </summary>
    public bool IsMenuVisible
    {
        get => GetValue(IsMenuVisibleProperty);
        set => SetValue(IsMenuVisibleProperty, value);
    }

    /// <summary>
    ///     The menu items.
    /// </summary>
    public static readonly StyledProperty<AvaloniaList<MenuItem>?> MenuItemsProperty =
        AvaloniaProperty.Register<Window, AvaloniaList<MenuItem>?>(nameof(MenuItems));

    /// <summary>
    ///     Gets or sets the value of the <see cref="MenuItemsProperty" />.
    /// </summary>
    public AvaloniaList<MenuItem>? MenuItems
    {
        get => GetValue(MenuItemsProperty);
        set => SetValue(MenuItemsProperty, value);
    }

    /// <summary>
    ///     Whether to enable minimize.
    /// </summary>
    public static readonly StyledProperty<bool> CanMinimizeProperty =
        AvaloniaProperty.Register<Window, bool>(nameof(CanMinimize), true);

    /// <summary>
    ///     Gets or sets the value of the <see cref="CanMinimizeProperty" />.
    /// </summary>
    public bool CanMinimize
    {
        get => GetValue(CanMinimizeProperty);
        set => SetValue(CanMinimizeProperty, value);
    }

    /// <summary>
    ///     Whether to show the title bar background.
    /// </summary>
    public static readonly StyledProperty<bool> ShowTitlebarBackgroundProperty =
        AvaloniaProperty.Register<Window, bool>(nameof(ShowTitlebarBackground), true);

    /// <summary>
    ///     Gets or sets the value of the <see cref="ShowTitlebarBackgroundProperty" />.
    /// </summary>
    public bool ShowTitlebarBackground
    {
        get => GetValue(ShowTitlebarBackgroundProperty);
        set => SetValue(ShowTitlebarBackgroundProperty, value);
    }

    /// <summary>
    ///     Whether to enable maximize.
    /// </summary>
    public static readonly StyledProperty<bool> CanMaximizeProperty =
        AvaloniaProperty.Register<Window, bool>(nameof(CanMaximize), true);

    /// <summary>
    ///     Gets or sets the value of the <see cref="CanMaximizeProperty" />.
    /// </summary>
    public bool CanMaximize
    {
        get => GetValue(CanMaximizeProperty);
        set => SetValue(CanMaximizeProperty, value);
    }

    /// <summary>
    ///     Whether to enable move.
    /// </summary>
    public static readonly StyledProperty<bool> CanMoveProperty =
        AvaloniaProperty.Register<Window, bool>(nameof(CanMove), true);

    /// <summary>
    ///     Gets or sets the value of the <see cref="CanMoveProperty" />.
    /// </summary>
    public bool CanMove
    {
        get => GetValue(CanMoveProperty);
        set => SetValue(CanMoveProperty, value);
    }

    /// <summary>
    ///     The controls on the right side of the title bar.
    /// </summary>
    public static readonly StyledProperty<Avalonia.Controls.Controls> RightWindowTitleBarControlsProperty =
        AvaloniaProperty.Register<Window, Avalonia.Controls.Controls>(nameof(RightWindowTitleBarControls),
            new Avalonia.Controls.Controls());

    /// <summary>
    ///     Gets or sets the value of the <see cref="RightWindowTitleBarControlsProperty" />.
    /// </summary>
    public Avalonia.Controls.Controls RightWindowTitleBarControls
    {
        get => GetValue(RightWindowTitleBarControlsProperty);
        set => SetValue(RightWindowTitleBarControlsProperty, value);
    }

    /// <summary>
    ///     These controls are displayed above all others and fill the entire window.
    ///     Useful for things like popups.
    /// </summary>
    public static readonly StyledProperty<Avalonia.Controls.Controls> HostsProperty =
        AvaloniaProperty.Register<Window, Avalonia.Controls.Controls>(nameof(Hosts), []);

    /// <summary>
    ///     These controls are displayed above all others and fill the entire window.
    /// </summary>
    public Avalonia.Controls.Controls Hosts
    {
        get => GetValue(HostsProperty);
        set => SetValue(HostsProperty, value);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Window" /> class.
    /// </summary>
    public Window()
    {
        MenuItems = new AvaloniaList<MenuItem>();
        RightWindowTitleBarControls = new Avalonia.Controls.Controls();
        Hosts = new Avalonia.Controls.Controls();
    }

    /// <summary>
    ///     Called when the window is loaded.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        if (desktop.MainWindow is Window window && window != this) Icon ??= window.Icon;
    }

    /// <summary>
    ///     Called when a property is changed.
    /// </summary>
    /// <param name="change">The event arguments.</param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == WindowStateProperty && change.NewValue is WindowState windowState)
            OnWindowStateChanged(windowState);
    }

    /// <summary>
    ///     Called when the template is applied.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        OnWindowStateChanged(WindowState);
        try
        {
            // Create handlers for buttons
            if (e.NameScope.Get<Button>("PART_MaximizeButton") is { } maximize)
            {
                maximize.Click += OnMaximizeButtonClicked;
                EnableWindowsSnapLayout(maximize);
            }

            if (e.NameScope.Get<Button>("PART_MinimizeButton") is { } minimize)
                minimize.Click += (_, _) => WindowState = WindowState.Minimized;

            if (e.NameScope.Get<Button>("PART_CloseButton") is { } close)
                close.Click += (_, _) => Close();

            if (e.NameScope.Get<Control>("PART_TitleBarBackground") is { } titleBar)
            {
                titleBar.PointerPressed += OnTitleBarPointerPressed;
                titleBar.DoubleTapped += OnMaximizeButtonClicked;
            }
        }
        catch
        {
            // ignored
        }
    }

    private void OnMaximizeButtonClicked(object? sender, RoutedEventArgs args)
    {
        if (!CanMaximize) return;
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    internal bool HasOpenDialog { get; set; }

    private void EnableWindowsSnapLayout(Button maximize)
    {
        var pointerOnMaxButton = false;
        var setter = typeof(Button).GetProperty("IsPointerOver");
        var proc = (IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam, ref bool handled) =>
        {
            switch (msg)
            {
                case 533:
                    if (!pointerOnMaxButton) break;
                    if (!CanMaximize) break;
                    WindowState = WindowState == WindowState.Maximized
                        ? WindowState.Normal
                        : WindowState.Maximized;
                    break;
                case 0x0084:
                    var point = new PixelPoint(
                        (short) (ToInt32(lParam) & 0xffff),
                        (short) (ToInt32(lParam) >> 16));
                    var desiredSize = maximize.DesiredSize;
                    var buttonLeftTop = maximize.PointToScreen(FlowDirection == FlowDirection.LeftToRight
                        ? new Point(desiredSize.Width, 0)
                        : new Point(0, 0));
                    var x = (buttonLeftTop.X - point.X) / RenderScaling;
                    var y = (point.Y - buttonLeftTop.Y) / RenderScaling;
                    if (new Rect(0, 0,
                            desiredSize.Width,
                            desiredSize.Height)
                        .Contains(new Point(x, y)))
                    {
                        if (HasOpenDialog) return (IntPtr) 9;
                        
                        setter?.SetValue(maximize, true);
                        pointerOnMaxButton = true;
                        handled = true;
                        return (IntPtr) 9;
                    }

                    pointerOnMaxButton = false;
                    setter?.SetValue(maximize, false);
                    break;
            }

            return IntPtr.Zero;

            static int ToInt32(IntPtr ptr)
            {
                return IntPtr.Size == 4
                    ? ptr.ToInt32()
                    : (int) (ptr.ToInt64() & 0xffffffff);
            }
        };

        Win32Properties.AddWndProcHookCallback(this, new Win32Properties.CustomWndProcHookCallback(proc));
    }

    private void OnWindowStateChanged(WindowState state)
    {
        if (state == WindowState.FullScreen)
            CanMaximize = CanResize = CanMove = false;
        if (state == WindowState.Maximized)
            Margin = new Thickness(7);
        else
            Margin = new Thickness(0);
    }

    private void OnTitleBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        BeginMoveDrag(e);
    }

    static Window()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            OnScreenKeyboard.Integrate();
        }
    }
}