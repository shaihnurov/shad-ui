using System;
using System.Threading.Tasks;

namespace ShadUI.Dialogs;

/// <summary>
///     Dialog options used to configure the dialog.
/// </summary>
public sealed class DialogOptions
{
    /// <summary>
    ///     Invoked when the dialog is closed.
    /// </summary>
    public Action<bool>? Callback { get; set; }

    /// <summary>
    ///     Invoked when the dialog is closed asynchronously.
    /// </summary>
    public Func<bool, Task>? AsyncCallback { get; set; }

    /// <summary>
    ///     Determines whether the dialog can be dismissed other than clicking/tapping an action button.
    /// </summary>
    public bool DismissibleDialog { get; set; }
}