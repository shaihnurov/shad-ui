using System;
using Avalonia.Controls;

namespace ShadUI.Dialogs;

/// <summary>
///     Builds a dialog.
/// </summary>
/// <typeparam name="TContext">The DataContext of a control</typeparam>
public sealed class DialogBuilder<TContext>
{
    private readonly DialogManager _manager;

    internal DialogBuilder(DialogManager manager)
    {
        _manager = manager;
    }

    internal Action? OnCancelCallback { get; set; }
    internal Action? OnSuccessCallback { get; set; }
    internal DialogOptions Options { get; } = new();

    private Control? _control;

    internal DialogBuilder<TContext> CreateDialog(TContext context)
    {
        if (!_manager.CustomDialogs.TryGetValue(typeof(TContext), out _control))
            throw new InvalidOperationException($"Custom dialog with {typeof(TContext)} is not registered.");

        _control.DataContext = context;
        return this;
    }

    internal void Show()
    {
        if (_control == null) throw new InvalidOperationException("Dialog control is not set.");

        if (OnSuccessCallback != null)
            _manager.OnSuccessCallbacks.Add(typeof(TContext), OnSuccessCallback);

        if (OnCancelCallback != null)
            _manager.OnCancelCallbacks.Add(typeof(TContext), OnCancelCallback);

        _manager.Show(_control, Options);
    }
}