using Avalonia.Controls;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace ShadUI;

/// <summary>
///     Dialog service for showing dialogs.
/// </summary>
public sealed class DialogManager
{
    internal event EventHandler<DialogShownEventArgs>? OnDialogShown;
    internal event EventHandler<DialogClosedEventArgs>? OnDialogClosed;

    internal readonly Dictionary<Control, DialogOptions> Dialogs = [];

    /// <summary>
    ///     Stores the mapping between dialog contexts and their <see cref="TaskCompletionSource{TResult}"/>,
    ///     used for asynchronously awaiting dialog results.
    /// </summary>
    private readonly ConcurrentDictionary<object, TaskCompletionSource<object?>> _resultSources = new();

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
                OnDialogClosed?.Invoke(this, new DialogClosedEventArgs { ReplaceExisting = true, Control = last.Key });
            }
        }

        Dialogs.TryAdd(control, options);
        OnDialogShown?.Invoke(this, new DialogShownEventArgs { Control = control, Options = options });
    }

    /// <summary>
    /// Shows a dialog asynchronously with the specified context and returns a result of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the dialog.</typeparam>
    /// <typeparam name="TContext">The type of the dialog context.</typeparam>
    /// <param name="context">The context object associated with the dialog.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the dialog operation.</param>
    /// <returns>A task that completes with the dialog result of type <typeparamref name="TResult"/>, or <c>null</c> if no result is provided.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a dialog with the specified context is already shown.</exception>
    public Task<TResult?> ShowDialogAsync<TResult, TContext>(TContext context, CancellationToken cancellationToken = default)
    {
        // Create a TaskCompletionSource to await the dialog result asynchronously.
        // Use RunContinuationsAsynchronously to ensure continuations run asynchronously.
        var tcs = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

        // Try to add the context and its TaskCompletionSource to the internal dictionary.
        // Throws if a dialog with the same context is already being shown.
        if (!_resultSources.TryAdd(context!, tcs))
            throw new InvalidOperationException("Dialog for this context is already shown.");

        // If a cancellation token is provided, register a callback to cancel the dialog.
        if (cancellationToken != default)
        {
            cancellationToken.Register(() =>
            {
                // Attempt to remove the TaskCompletionSource for the context and cancel the task.
                if (_resultSources.TryRemove(context!, out var source))
                {
                    source.TrySetCanceled();
                    // Close the dialog associated with the context.
                    Close(context!);
                }
            });
        }

        // Create and show the dialog for the given context, making it dismissible by default.
        this.CreateDialog(context).Dismissible().Show();

        // Return a task that completes when the dialog finishes.
        // On completion, remove the context from the dictionary.
        // If the task was cancelled, throw TaskCanceledException.
        // Otherwise, cast the result to TResult or return default.
        return tcs.Task.ContinueWith(task =>
        {
            _resultSources.TryRemove(context!, out _);

            if (task.IsCanceled)
                throw new TaskCanceledException(task);

            return task.Result is TResult result ? result : default;
        }, TaskScheduler.Current);
    }

    internal void CloseDialog(Control control)
    {
        Dialogs.Remove(control);

        OnDialogClosed?.Invoke(this, new DialogClosedEventArgs
        {
            ReplaceExisting = Dialogs.Count > 0,
            Control = control
        }
        );
    }

    internal void OpenLast()
    {
        if (Dialogs.Count == 0) return;

        var lastDialog = Dialogs.Last();
        OnDialogShown?.Invoke(this, new DialogShownEventArgs { Control = lastDialog.Key, Options = lastDialog.Value });
    }

    internal void RemoveLast()
    {
        if (Dialogs.Count == 0) return;

        var lastDialog = Dialogs.Last();
        CloseDialog(lastDialog.Key);

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
    ///     Closes the dialog associated with the specified context and invokes the appropriate callbacks.
    /// </summary>
    /// <typeparam name="TContext">The type of the DataContext associated with the dialog.</typeparam>
    /// <param name="context">The DataContext of the dialog to close.</param>
    /// <param name="options">Optional parameters for closing the dialog.</param>
    public void Close<TContext>(TContext context, CloseDialogOptions? options = null)
    {
        var clearAll = options?.ClearAll ?? false;
        var dialogs = Dialogs.Where(x => Equals(x.Key.DataContext, context)).ToList();

        if (clearAll) RemoveAll();

        foreach (var dialog in dialogs) CloseDialog(dialog.Key);

        var success = options?.Success ?? false;
        InvokeCallBacks(typeof(TContext), success);

        // Try to remove the TaskCompletionSource associated with the dialog context.
        // If found, set the task result or cancel it based on the provided options.
        // If 'Canceled' is true, the task is marked as canceled.
        // Otherwise, the task is completed with the specified result (which may be null).
        if (_resultSources.TryRemove(context!, out var tcs))
        {
            if (options != null && options.Canceled)
                tcs.TrySetCanceled();
            else
                tcs.TrySetResult(options?.Result);
        }

        if (!clearAll) OpenLast();
    }

    private void RemoveAll()
    {
        Dialogs.Clear();
        OnSuccessAsyncCallbacks.Clear();
        OnSuccessCallbacks.Clear();
        OnCancelAsyncCallbacks.Clear();
        OnCancelCallbacks.Clear();
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
}

internal sealed class DialogShownEventArgs : EventArgs
{
    public Control Control { get; set; } = null!;
    public DialogOptions Options { get; set; } = null!;
}

internal sealed class DialogClosedEventArgs : EventArgs
{
    public bool ReplaceExisting { get; set; }
    public Control Control { get; set; } = null!;

    /// <summary>
    ///     The result returned when the dialog is closed.
    ///     This value is passed through <see cref="CloseDialogOptions.Result"/>.
    /// </summary>
    public object? Result { get; set; }
}