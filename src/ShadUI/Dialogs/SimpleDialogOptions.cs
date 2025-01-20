using System;
using System.Threading.Tasks;

namespace ShadUI.Dialogs;

/// <summary>
///     Dialog options used to configure the dialog.
/// </summary>
public sealed class SimpleDialogOptions
{
    /// <summary>
    ///     The title of the dialog.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     The message of the dialog.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    ///     Text content of the primary button. The default value is "OK".
    /// </summary>
    public string PrimaryButtonText { get; set; } = "OK";

    /// <summary>
    ///     Text content of the secondary button.
    /// </summary>
    public string SecondaryButtonText { get; set; } = "";

    /// <summary>
    ///     Text content of the tertiary button.
    /// </summary>
    public string TertiaryButtonText { get; set; } = "";

    /// <summary>
    ///     Text content of the cancel button.
    /// </summary>
    public string CancelButtonText { get; set; } = "";

    /// <summary>
    ///     Defines the style of the primary button. The default value is <see cref="SimpleDialogButtonStyle.Primary" />.
    /// </summary>
    public SimpleDialogButtonStyle PrimaryButtonStyle { get; set; } = SimpleDialogButtonStyle.Primary;

    /// <summary>
    ///     Defines the style of the secondary button. The default value is <see cref="SimpleDialogButtonStyle.Secondary" />.
    /// </summary>
    public SimpleDialogButtonStyle SecondaryButtonStyle { get; set; } = SimpleDialogButtonStyle.Secondary;

    /// <summary>
    ///     Defines the style of the tertiary button. The default value is <see cref="SimpleDialogButtonStyle.Outline" />.
    /// </summary>
    public SimpleDialogButtonStyle TertiaryButtonStyle { get; set; } = SimpleDialogButtonStyle.Outline;

    /// <summary>
    ///     Defines the style of the cancel button. The default value is <see cref="SimpleDialogButtonStyle.Outline" />.
    /// </summary>
    public SimpleDialogButtonStyle CancelButtonStyle { get; set; } = SimpleDialogButtonStyle.Outline;

    /// <summary>
    ///     Defines the maximum width of the dialog.
    /// </summary>
    public double? MaxWidth { get; set; }

    /// <summary>
    ///     Invoked when the dialog is closed.
    /// </summary>
    public Action<SimpleDialogAction>? Callback { get; set; }

    /// <summary>
    ///     Invoked when the dialog is closed asynchronously.
    /// </summary>
    public Func<SimpleDialogAction, Task>? AsyncCallback { get; set; }

    /// <summary>
    ///     Determines whether the dialog can be dismissed other than clicking/tapping an action button.
    /// </summary>
    public bool DismissibleDialog { get; set; }
}