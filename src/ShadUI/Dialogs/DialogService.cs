using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Window = ShadUI.Controls.Window;

namespace ShadUI.Dialogs;

/// <summary>
///     Dialog service for showing dialogs.
/// </summary>
/// <param name="hostName"></param>
public sealed class DialogService(string hostName = "PART_DialogHost")
{
    private Window? _window;
    private DialogHost? _dialogHost;

    private Window? GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            return desktop.MainWindow as Window;
        return null;
    }

    /// <summary>
    ///     Shows a simple dialog with the provided options.
    /// </summary>
    /// <param name="opts">Dialog options</param>
    /// <exception cref="InvalidOperationException">
    ///     Throws when <see cref="Window" /> or the <see cref="DialogHost" /> is not
    ///     available.
    /// </exception>
    public void Show(SimpleDialogOptions opts)
    {
        _window ??= GetMainWindow();

        if (_window == null)
            throw new InvalidOperationException(
                "Main window could not be found. Make sure the application is properly initialized.");

        var dialog = new SimpleDialog(new SimpleDialogContext(this, opts));
        if (_dialogHost != null)
        {
            _dialogHost.Dialog = dialog;
            _dialogHost.Dismissible = opts.DismissibleDialog;
            _dialogHost.IsDialogOpen = true;
            _window.HasOpenDialog = true;
            return;
        }

        _dialogHost = _window.Hosts.FirstOrDefault(x => x.Name == hostName) as DialogHost;
        if (_dialogHost == null)
            throw new InvalidOperationException(
                "DialogHost could not be found in the main window. Make sure it is properly initialized.");

        _dialogHost.Dialog = dialog;
        _dialogHost.Dismissible = opts.DismissibleDialog;
        _dialogHost.IsDialogOpen = true;
        _window.HasOpenDialog = true;
    }

    /// <summary>
    ///     Closes the dialog.
    /// </summary>
    public void Close()
    {
        if (_window == null || _dialogHost == null) return;

        _dialogHost.IsDialogOpen = false;
        _dialogHost.Dialog = null;
        _window.HasOpenDialog = false;
    }

    private readonly Dictionary<Type, Control> _customDialogs = [];

    /// <summary>
    ///     Registers a custom dialog view with its DataContext type.
    /// </summary>
    /// <param name="view">The actual view object</param>
    /// <typeparam name="TView">The type of view</typeparam>
    /// <typeparam name="TContext">The type of DataContext</typeparam>
    /// <returns></returns>
    public DialogService Register<TView, TContext>(TView view) where TView : Control
    {
        _customDialogs.Add(typeof(TContext), view);
        return this;
    }

    private readonly Dictionary<Type, Action<bool>> _callbacks = [];
    private readonly Dictionary<Type, Func<bool, Task>> _asyncCallbacks = [];

    /// <summary>
    ///     Shows a custom dialog with the provided options.
    /// </summary>
    /// <param name="context">The actual DataContext object</param>
    /// <param name="opts">The dialog options</param>
    /// <typeparam name="TContext">The type of DataContext</typeparam>
    /// <exception cref="InvalidOperationException">
    ///     Throws when the <see cref="DialogOptions.Callback" /> and
    ///     <see cref="DialogOptions.AsyncCallback" /> are both provided or the MainWindow or the <see cref="DialogHost" />
    ///     cannot be found. It will also throw when the custom dialog with the context is not registered.
    /// </exception>
    public void Show<TContext>(TContext context, DialogOptions opts)
    {
        if (opts.Callback is not null && opts.AsyncCallback is not null)
            throw new InvalidOperationException("Both synchronous and asynchronous callbacks are provided.");

        //ensure callbacks are cleared before showing a new dialog
        _callbacks.Clear();
        _asyncCallbacks.Clear();

        if (opts.Callback is not null) _callbacks[typeof(TContext)] = opts.Callback;
        if (opts.AsyncCallback is not null) _asyncCallbacks[typeof(TContext)] = opts.AsyncCallback;

        if (!_customDialogs.TryGetValue(typeof(TContext), out var view))
        {
            throw new InvalidOperationException($"Custom dialog with {typeof(TContext)} is not registered."); 
        }

        _window ??= GetMainWindow();

        if (_window == null)
            throw new InvalidOperationException(
                "Main window could not be found. Make sure the application is properly initialized.");

        if (_dialogHost != null)
        {
            view.DataContext = context;
            _dialogHost.Dialog = view;
            _dialogHost.Dismissible = opts.DismissibleDialog;
            _dialogHost.IsDialogOpen = true;
            _window.HasOpenDialog = true;
            return;
        }

        _dialogHost = _window.Hosts.FirstOrDefault(x => x.Name == hostName) as DialogHost;
        if (_dialogHost == null)
            throw new InvalidOperationException(
                "DialogHost could not be found in the main window. Make sure it is properly initialized.");

        view.DataContext = context;
        _dialogHost.Dialog = view;
        _dialogHost.Dismissible = opts.DismissibleDialog;
        _dialogHost.IsDialogOpen = true;
        _window.HasOpenDialog = true;
    }

    /// <summary>
    ///     Closes the dialog and invokes the callback.
    /// </summary>
    /// <param name="success">Returns whether the action is successful or not</param>
    /// <typeparam name="TContext">The registered DataContext</typeparam>
    public void Close<TContext>(bool success)
    {
        if (_callbacks.TryGetValue(typeof(TContext), out var callback))
            callback.Invoke(success);
        if (_asyncCallbacks.TryGetValue(typeof(TContext), out var asyncCallback))
            asyncCallback.Invoke(success);

        _callbacks.Remove(typeof(TContext));
        _asyncCallbacks.Remove(typeof(TContext));

        Close();
    }
}