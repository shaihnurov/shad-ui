using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace ShadUI.Dialogs;

/// <summary>
///     Dialog service for showing dialogs.
/// </summary>
public sealed class DialogManager
{
    internal event EventHandler<DialogShownEventArgs>? OnDialogShown;
    internal event EventHandler<DialogClosedEventArgs>? OnDialogClosed;

    private readonly Dictionary<Control, DialogOptions> _dialogs = [];

    /// <summary>
    ///     Shows a dialog with the provided options.
    /// </summary>
    /// <param name="control">Control to be shown as dialog</param>
    /// <param name="options">Dialog options</param>
    internal void Show(Control control, DialogOptions options)
    {
        if (_dialogs.Count > 0)
        {
            var last = _dialogs.Last();
            if (last.Key != control)
            {
                OnDialogClosed?.Invoke(this, new DialogClosedEventArgs { Control = last.Key });
            }
        }

        var existing = _dialogs.FirstOrDefault(x => x.Key == control || x.Key.GetType() == control.GetType());
        if (existing.Key is null) _dialogs.TryAdd(control, options);

        var current = existing.Key ?? control;
        OnDialogShown?.Invoke(this, new DialogShownEventArgs { Control = current, Options = options });
    }

    /// <summary>
    ///     Closes the dialog.
    /// </summary>
    public void Close(Control control)
    {
        _dialogs.Remove(control);
        OnDialogClosed?.Invoke(this, new DialogClosedEventArgs { Control = control });

        if (_dialogs.Count == 0) return;

        var lastDialog = _dialogs.Last();
        Show(lastDialog.Key, lastDialog.Value);
    }

    internal void RemoveLast()
    {
        if (_dialogs.Count == 0) return;

        var lastDialog = _dialogs.Last();
        Close(lastDialog.Key);
        var contextType = lastDialog.Key.DataContext?.GetType();
        if (contextType is not null) InvokeCallBacks(contextType, false);
    }

    internal readonly Dictionary<Type, Type> CustomDialogs = [];

    /// <summary>
    ///     Registers a custom dialog view with its DataContext type.
    /// </summary>
    /// <typeparam name="TView">The type of view</typeparam>
    /// <typeparam name="TContext">The type of DataContext</typeparam>
    /// <returns></returns>
    public DialogManager Register<TView, TContext>() where TView : Control
    {
        CustomDialogs.Add(typeof(TContext), typeof(TView));
        return this;
    }

    internal readonly Dictionary<Type, Action> OnCancelCallbacks = [];
    internal readonly Dictionary<Type, Func<Task>> OnCancelAsyncCallbacks = [];
    internal readonly Dictionary<Type, Action> OnSuccessCallbacks = [];
    internal readonly Dictionary<Type, Func<Task>> OnSuccessAsyncCallbacks = [];

    /// <summary>
    ///     Closes the dialog and invokes the callback.
    /// </summary>
    /// <param name="success">Returns whether the action is successful or not</param>
    /// <typeparam name="TContext">The registered DataContext</typeparam>
    public void Close<TContext>(bool success)
    {
        if (!CustomDialogs.TryGetValue(typeof(TContext), out var control))
        {
            throw new InvalidOperationException($"Dialog with {typeof(TContext)} is not registered.");
        }

        InvokeCallBacks(typeof(TContext), success);

        var dialogs = _dialogs
            .Where(x => x.Key.GetType() == control &&
                        x.Key.DataContext?.GetType() == typeof(TContext))
            .ToList();

        foreach (var dialog in dialogs) Close(dialog.Key);
    }

    private void InvokeCallBacks(Type type, bool success)
    {
        if (OnSuccessCallbacks.Remove(type, out var successCallback) && success)
        {
            successCallback?.Invoke();
        }

        if (OnSuccessAsyncCallbacks.Remove(type, out var successAsyncCallback) && success)
        {
            successAsyncCallback?.Invoke();
        }

        if (OnCancelCallbacks.Remove(type, out var cancelCallback) && !success)
        {
            cancelCallback?.Invoke();
        }

        if (OnCancelAsyncCallbacks.Remove(type, out var cancelAsyncCallback) && !success)
        {
            cancelAsyncCallback?.Invoke();
        }
    }

    internal event EventHandler<bool>? AllowDismissChanged;

    /// <summary>
    ///     Disables the ability to dismiss dialogs. This overrides the <see cref="DialogHost.Dismissible" /> property of the
    ///     <see cref="DialogHost" />.
    /// </summary>
    public void PreventDismissal()
    {
        AllowDismissChanged?.Invoke(this, false);
    }

    /// <summary>
    ///     Enables the ability to dismiss dialogs. This overrides the <see cref="DialogHost.Dismissible" /> property of the
    ///     <see cref="DialogHost" />.
    /// </summary>
    public void AllowDismissal()
    {
        AllowDismissChanged?.Invoke(this, true);
    }

    /// <summary>
    ///     Removes all dialogs including all associated callbacks.
    /// </summary>
    public void RemoveAll()
    {
        _dialogs.Clear();
        OnSuccessAsyncCallbacks.Clear();
        OnSuccessCallbacks.Clear();
        OnCancelAsyncCallbacks.Clear();
        OnCancelCallbacks.Clear();
    }
}

internal sealed class DialogShownEventArgs : EventArgs
{
    public Control Control { get; set; } = null!;
    public DialogOptions Options { get; set; } = null!;
}

internal sealed class DialogClosedEventArgs : EventArgs
{
    public Control Control { get; set; } = null!;
}