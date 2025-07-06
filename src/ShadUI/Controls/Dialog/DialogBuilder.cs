using Avalonia.Controls;
using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace ShadUI;

/// <summary>
///     Builds a dialog.
/// </summary>
/// <typeparam name="TContext">The DataContext of a control</typeparam>
public sealed class DialogBuilder<TContext>
{
    private readonly DialogManager _manager;

    /// <summary>
    ///     Gets the dialog context (DataContext of the dialog control).
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the dialog control has not been created or set.
    /// </exception>
    private TContext Context => (TContext)((_control?.DataContext) ?? throw new InvalidOperationException("Dialog control is not set."));

    internal DialogBuilder(DialogManager manager)
    {
        _manager = manager;
    }

    internal Action? OnCancelCallback { get; set; }
    internal Func<Task>? OnCancelAsyncCallback { get; set; }
    internal Action? OnSuccessCallback { get; set; }
    internal Func<Task>? OnSuccessAsyncCallback { get; set; }
    internal DialogOptions Options { get; } = new();

    private Control? _control;

    internal DialogBuilder<TContext> CreateDialog(TContext context)
    {
        if (!_manager.CustomDialogs.TryGetValue(typeof(TContext), out var type))
        {
            throw new InvalidOperationException($"Custom dialog with {typeof(TContext)} is not registered.");
        }

        _control = Activator.CreateInstance(type) as Control;

        if (_control == null) throw new InvalidOperationException("Dialog control is not set.");

        _control.DataContext = context;
        return this;
    }

    internal void Show()
    {
        if (_control == null) throw new InvalidOperationException("Dialog control is not set.");

        if (OnSuccessCallback != null)
        {
            _manager.OnSuccessCallbacks.TryAdd(typeof(TContext), OnSuccessCallback);
        }

        if (OnSuccessAsyncCallback != null)
        {
            _manager.OnSuccessAsyncCallbacks.TryAdd(typeof(TContext), OnSuccessAsyncCallback);
        }

        if (OnCancelCallback != null)
        {
            _manager.OnCancelCallbacks.TryAdd(typeof(TContext), OnCancelCallback);
        }

        if (OnCancelAsyncCallback != null)
        {
            _manager.OnCancelAsyncCallbacks.TryAdd(typeof(TContext), OnCancelAsyncCallback);
        }

        _manager.Show(_control, Options);
    }

    /// <summary>
    ///     Shows the dialog and returns the result asynchronously.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned from the dialog.</typeparam>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the asynchronous operation,
    ///     containing the result of type <typeparamref name="TResult"/> or <c>null</c>.
    /// </returns>
    public Task<TResult?> ShowDialogAsync<TResult>()
    {
        var context = Context;
        Show(); // automatic call Show
        return _manager.ShowDialogAsync<TResult, TContext>(context);
    }

    /// <summary>
    ///     Shows the dialog with support for cancellation and returns the result asynchronously.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned from the dialog.</typeparam>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the asynchronous operation,
    ///     containing the result of type <typeparamref name="TResult"/> or <c>null</c>.
    /// </returns>
    public Task<TResult?> ShowDialogAsync<TResult>(CancellationToken cancellationToken)
    {
        var context = Context;
        Show(); // automatic call Show
        return _manager.ShowDialogAsync<TResult, TContext>(context, cancellationToken);
    }
}