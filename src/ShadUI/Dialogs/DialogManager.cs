using System;
using System.Collections.Generic;
using Avalonia.Controls;

namespace ShadUI.Dialogs;

/// <summary>
///     Dialog service for showing dialogs.
/// </summary>
public sealed class DialogManager
{
    internal event EventHandler<DialogShownEventArgs>? OnDialogShown;
    internal event EventHandler<DialogClosedEventArgs>? OnDialogClosed;

    /// <summary>
    ///     Shows a dialog with the provided options.
    /// </summary>
    /// <param name="control">Control to be shown as dialog</param>
    /// <param name="options">Dialog options</param>
    internal void Show(Control control, DialogOptions options)
    {
        OnDialogShown?.Invoke(this, new DialogShownEventArgs { Control = control, Options = options });
    }

    /// <summary>
    ///     Closes the dialog.
    /// </summary>
    public void Close(Control control)
    {
        OnDialogClosed?.Invoke(this, new DialogClosedEventArgs { Control = control });
    }

    internal readonly Dictionary<Type, Control> CustomDialogs = [];

    /// <summary>
    ///     Registers a custom dialog view with its DataContext type.
    /// </summary>
    /// <param name="view">The actual view object</param>
    /// <typeparam name="TView">The type of view</typeparam>
    /// <typeparam name="TContext">The type of DataContext</typeparam>
    /// <returns></returns>
    public DialogManager Register<TView, TContext>(TView view) where TView : Control
    {
        CustomDialogs.Add(typeof(TContext), view);
        return this;
    }

    internal readonly Dictionary<Type, Action> OnCancelCallbacks = [];
    internal readonly Dictionary<Type, Action> OnSuccessCallbacks = [];

    /// <summary>
    ///     Closes the dialog and invokes the callback.
    /// </summary>
    /// <param name="success">Returns whether the action is successful or not</param>
    /// <typeparam name="TContext">The registered DataContext</typeparam>
    public void Close<TContext>(bool success)
    {
        if (!CustomDialogs.TryGetValue(typeof(TContext), out var control))
            throw new InvalidOperationException($"Dialog with {typeof(TContext)} is not registered.");

        if (OnSuccessCallbacks.TryGetValue(typeof(TContext), out var successCallback))
        {
            OnSuccessCallbacks.Remove(typeof(TContext));
            if(success) successCallback?.Invoke();
        }

        if (OnCancelCallbacks.TryGetValue(typeof(TContext), out var cancelCallback))
        {
            OnCancelCallbacks.Remove(typeof(TContext));
            if(!success) cancelCallback?.Invoke();
        }

        Close(control);
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