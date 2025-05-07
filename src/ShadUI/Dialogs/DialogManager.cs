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

    internal readonly Dictionary<Control, DialogOptions> Dialogs = [];

    /// <summary>
    ///     Shows a dialog with the provided options.
    /// </summary>
    /// <param name="control">Control to be shown as dialog</param>
    /// <param name="options">Dialog options</param>
    internal void Show(Control control, DialogOptions options)
    {
        if (Dialogs.Count > 0)
        {
            if (control is SimpleDialog simple)
            {
                var existingSimpleDialog = Dialogs.FirstOrDefault(x => x.Key is SimpleDialog d && d.Id == simple.Id)
                    .Key;

                if (existingSimpleDialog is not null) return;
            }

            var existingCustomDialog =
                Dialogs.FirstOrDefault(x =>
                    x.Key.DataContext?.GetType() == control.DataContext?.GetType()).Key;
            if (existingCustomDialog is not null) return;

            var last = Dialogs.Last();
            if (last.Key != control)
            {
                OnDialogClosed?.Invoke(this, new DialogClosedEventArgs { Control = last.Key });
            }
        }

        Dialogs.TryAdd(control, options);
        OnDialogShown?.Invoke(this, new DialogShownEventArgs { Control = control, Options = options });
    }

    internal void CloseAndTryOpenLast(Control control)
    {
        Dialogs.Remove(control);
        OnDialogClosed?.Invoke(this, new DialogClosedEventArgs { Control = control });

        if (Dialogs.Count == 0) return;

        var lastDialog = Dialogs.Last();
        OnDialogShown?.Invoke(this, new DialogShownEventArgs { Control = lastDialog.Key, Options = lastDialog.Value });
    }

    internal void RemoveLast()
    {
        if (Dialogs.Count == 0) return;

        var lastDialog = Dialogs.Last();
        CloseAndTryOpenLast(lastDialog.Key);
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
    /// /// <remarks>
    ///     This method is deprecated and will be removed in the next release. Use the overload with the context parameter instead.
    /// </remarks>
    [Obsolete("This method is deprecated and will be removed in the major next release. Use the overload with the context parameter instead.")]
    public void Close<TContext>(bool success = false)
    {
        if (!CustomDialogs.TryGetValue(typeof(TContext), out var control))
        {
            throw new InvalidOperationException($"Dialog with {typeof(TContext)} is not registered.");
        }

        InvokeCallBacks(typeof(TContext), success);

        var dialogs = Dialogs.Where(x => x.Key.GetType() == control && x.Key.DataContext?.GetType() == typeof(TContext));
        foreach (var dialog in dialogs) CloseAndTryOpenLast(dialog.Key);
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

    /// <summary>
    ///     Closes the dialog and invokes the callback.
    /// </summary>
    /// <param name="context">The actual context</param>
    /// <param name="success">Returns whether the action is successful or not. Default is false.</param>
    /// <typeparam name="TContext">The registered DataContext type</typeparam>
    public void Close<TContext>(TContext context, bool success = false)
    {
        InvokeCallBacks(typeof(TContext), success);
        var dialogs = Dialogs.Where(x => Equals(x.Key.DataContext, context));
        foreach (var dialog in dialogs) CloseAndTryOpenLast(dialog.Key);
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
        Dialogs.Clear();
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