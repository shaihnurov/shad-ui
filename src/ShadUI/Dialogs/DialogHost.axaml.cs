using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Reactive;
using Window = ShadUI.Controls.Window;

namespace ShadUI.Dialogs;

/// <summary>
///     Dialog host control that manages the display and lifecycle of dialogs within a window.
/// </summary>
public class DialogHost : TemplatedControl
{
    /// <summary>
    ///     Defines the <see cref="Owner" /> property.
    /// </summary>
    public static readonly StyledProperty<Window?> OwnerProperty =
        AvaloniaProperty.Register<DialogHost, Window?>(nameof(Owner));

    /// <summary>
    ///     Gets or sets the owner window of the dialog host.
    /// </summary>
    public Window? Owner
    {
        get => GetValue(OwnerProperty);
        set => SetValue(OwnerProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="Manager" /> property.
    /// </summary>
    public static readonly StyledProperty<DialogManager?> ManagerProperty =
        AvaloniaProperty.Register<DialogHost, DialogManager?>(nameof(Manager));

    /// <summary>
    ///     Gets or sets the dialog manager responsible for handling dialog operations.
    /// </summary>
    public DialogManager? Manager
    {
        get => GetValue(ManagerProperty);
        set => SetValue(ManagerProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="Dialog" /> property.
    /// </summary>
    internal static readonly StyledProperty<object?> DialogProperty =
        AvaloniaProperty.Register<DialogHost, object?>(nameof(Dialog));

    /// <summary>
    ///     Gets or sets the current dialog content.
    /// </summary>
    internal object? Dialog
    {
        get => GetValue(DialogProperty);
        set => SetValue(DialogProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="IsDialogOpen" /> property.
    /// </summary>
    internal static readonly StyledProperty<bool> IsDialogOpenProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(IsDialogOpen));

    /// <summary>
    ///     Gets or sets whether a dialog is currently open.
    /// </summary>
    internal bool IsDialogOpen
    {
        get => GetValue(IsDialogOpenProperty);
        set => SetValue(IsDialogOpenProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="DialogMaxWidth" /> property.
    /// </summary>
    internal static readonly StyledProperty<double?> DialogMaxWidthProperty =
        AvaloniaProperty.Register<DialogHost, double?>(nameof(DialogMaxWidth));

    /// <summary>
    ///     Gets or sets the maximum width of the dialog.
    /// </summary>
    internal double? DialogMaxWidth
    {
        get => GetValue(DialogMaxWidthProperty);
        set => SetValue(DialogMaxWidthProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="DialogMinWidth" /> property.
    /// </summary>
    internal static readonly StyledProperty<double?> DialogMinWidthProperty =
        AvaloniaProperty.Register<DialogHost, double?>(nameof(DialogMinWidth));

    /// <summary>
    ///     Gets or sets the minimum width of the dialog.
    /// </summary>
    internal double? DialogMinWidth
    {
        get => GetValue(DialogMinWidthProperty);
        set => SetValue(DialogMinWidthProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="Dismissible" /> property.
    /// </summary>
    internal static readonly StyledProperty<bool> DismissibleProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(Dismissible), true);

    /// <summary>
    ///     Gets or sets whether the dialog can be dismissed.
    /// </summary>
    internal bool Dismissible
    {
        get => GetValue(DismissibleProperty);
        set => SetValue(DismissibleProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="CanDismissWithBackgroundClick" /> property.
    /// </summary>
    internal static readonly StyledProperty<bool> CanDismissWithBackgroundClickProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(CanDismissWithBackgroundClick), true);

    /// <summary>
    ///     Gets or sets whether the dialog can be dismissed by clicking the background.
    /// </summary>
    internal bool CanDismissWithBackgroundClick
    {
        get => GetValue(CanDismissWithBackgroundClickProperty);
        set => SetValue(CanDismissWithBackgroundClickProperty, value);
    }

    /// <summary>
    ///     Called when the control template is applied to set up event handlers and animations.
    /// </summary>
    /// <param name="e">The template applied event arguments.</param>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (e.NameScope.Find<Border>("PART_DialogBackground") is { } background)
            background.PointerPressed += (_, _) =>
            {
                if (CanDismissWithBackgroundClick) CloseDialog();
            };

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

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
            {
                MainWindow: not null
            } desktop)
            desktop.MainWindow.BeginMoveDrag(e);
    }

    private void OnMaximizeButtonClicked(object? sender, RoutedEventArgs args)
    {
        if (Owner is not null && Owner.CanMaximize)
            Owner.WindowState = Owner.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
    }

    private void CloseDialog()
    {
        if (!Dismissible) return;

        IsDialogOpen = false;

        if (Manager is not null)
        {
            Manager.OnCancelCallbacks.Clear();
            Manager.OnSuccessCallbacks.Clear();
        }

        if (Owner is not null)
            Owner.HasOpenDialog = false;
    }

    static DialogHost()
    {
        ManagerProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<DialogManager?>>(x =>
                OnManagerPropertyChanged(x.Sender, x)));
    }

    private static void OnManagerPropertyChanged(AvaloniaObject sender,
        AvaloniaPropertyChangedEventArgs propChanged)
    {
        if (sender is not DialogHost host)
            throw new NullReferenceException("Dependency object is not of valid type " + nameof(DialogHost));
        if (propChanged.OldValue is DialogManager oldManager)
        {
            oldManager.AllowDismissChanged -= host.AllowDismissChanged;
            host.DetachManagerEvents(oldManager);
        }

        if (propChanged.NewValue is DialogManager newManager)
        {
            newManager.AllowDismissChanged += host.AllowDismissChanged;
            host.AttachManagerEvents(newManager);
        }
    }

    private void AllowDismissChanged(object sender, bool e)
    {
        Dismissible = e;
    }

    private void AttachManagerEvents(DialogManager manager)
    {
        manager.OnDialogShown += ManagerOnDialogShown;
        manager.OnDialogClosed += ManagerOnDialogClosed;
    }

    private void DetachManagerEvents(DialogManager manager)
    {
        manager.OnDialogShown -= ManagerOnDialogShown;
        manager.OnDialogClosed -= ManagerOnDialogClosed;
    }

    private void ManagerOnDialogShown(object sender, DialogShownEventArgs e)
    {
        Dialog = e.Control;
        Dismissible = e.Options.Dismissible;

        if (e.Options.MaxWidth > 0)
            DialogMaxWidth = e.Options.MaxWidth;

        if (e.Options.MinWidth > 0)
            DialogMinWidth = e.Options.MinWidth;

        if (Owner is not null)
            Owner.HasOpenDialog = true;

        IsDialogOpen = true;
    }

    private void ManagerOnDialogClosed(object sender, DialogClosedEventArgs e)
    {
        if (e.Control != Dialog) return;

        IsDialogOpen = false;

        if (Owner is not null)
            Owner.HasOpenDialog = false;
    }
}