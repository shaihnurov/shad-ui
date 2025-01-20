using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Rendering.Composition;
using ShadUI.Utilities;

namespace ShadUI.Dialogs;

/// <summary>
/// Dialog host control.
/// </summary>
public class DialogHost : TemplatedControl
{
    /// <summary>
    /// Represents an instance of <see cref="DialogHost" />.
    /// </summary>
    public DialogHost()
    {
        Name = "PART_DialogHost";
    }

    internal static readonly StyledProperty<object?> DialogProperty =
        AvaloniaProperty.Register<DialogHost, object?>(nameof(Dialog));

    internal object? Dialog
    {
        get => GetValue(DialogProperty);
        set => SetValue(DialogProperty, value);
    }

    internal static readonly StyledProperty<bool> IsDialogOpenProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(IsDialogOpen));

    internal bool IsDialogOpen
    {
        get => GetValue(IsDialogOpenProperty);
        set => SetValue(IsDialogOpenProperty, value);
    }

    internal static readonly StyledProperty<bool> DismissibleProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(Dismissible), true);

    internal bool Dismissible
    {
        get => GetValue(DismissibleProperty);
        set => SetValue(DismissibleProperty, value);
    }

    internal static readonly StyledProperty<bool> CanDismissWithBackgroundClickProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(CanDismissWithBackgroundClick), true);

    internal bool CanDismissWithBackgroundClick
    {
        get => GetValue(CanDismissWithBackgroundClickProperty);
        set => SetValue(CanDismissWithBackgroundClickProperty, value);
    }

    /// <summary>
    /// Called when the control is applied to a control template.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (e.NameScope.Find<Border>("PART_DialogBackground") is { } background)
        {
            background.Loaded += (_, _) =>
            {
                var element = ElementComposition.GetElementVisual(background);
                if (element != null)
                    CompositionAnimationHelper.MakeOpacityAnimated(element, 400);
            };
            background.PointerPressed += (_, _) =>
            {
                if (CanDismissWithBackgroundClick) CloseDialog();
            };
        }

        if (e.NameScope.Find<Border>("PART_TitleBar") is { } titleBar)
        {
            titleBar.PointerPressed += OnTitleBarPointerPressed;
            titleBar.DoubleTapped += OnMaximizeButtonClicked;
        }

        if (e.NameScope.Find<Button>("PART_CloseButton") is { } closeButton)
            closeButton.Click += (_, _) => CloseDialog();
    }

    private void OnTitleBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: not null } desktop)
            desktop.MainWindow.BeginMoveDrag(e);
    }

    private static void OnMaximizeButtonClicked(object? sender, RoutedEventArgs args)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime
            {
                MainWindow: not null
            } desktop) return;

        if (desktop.MainWindow is ShadUI.Controls.Window { CanMaximize: true } window)
            window.WindowState = window.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
    }

    private void CloseDialog()
    {
        if (!Dismissible) return;

        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime
            {
                MainWindow: not null
            } desktop) return;
        IsDialogOpen = false;
        Dialog = null;

        if (desktop.MainWindow is ShadUI.Controls.Window window)
            window.HasOpenDialog = false;
    }
}