// ReSharper disable once CheckNamespace
namespace ShadUI;

/// <summary>
///     Options for closing a dialog.
/// </summary>
public sealed class CloseDialogOptions
{
    /// <summary>
    ///     Indicates whether the dialog was closed successfully.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    ///     Specifies whether all dialogs should be cleared when closing this dialog.
    /// </summary>
    public bool ClearAll { get; set; }

    /// <summary>
    ///     The result object to return when the dialog is closed.
    /// </summary>
    public object? Result { get; set; }

    /// <summary>
    ///     Indicates whether the dialog was canceled (e.g., via cancellation token).
    /// </summary>
    public bool Canceled { get; set; } = false;
}