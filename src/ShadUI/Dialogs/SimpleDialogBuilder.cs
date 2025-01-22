using System;

namespace ShadUI.Dialogs;

/// <summary>
///     Builds a simple dialog.
/// </summary>
public sealed class SimpleDialogBuilder
{
    private readonly DialogManager _manager;
    private SimpleDialog? _dialog;

    internal SimpleDialogBuilder(DialogManager manager)
    {
        _manager = manager;
    }

    internal string PrimaryButtonText { get; set; } = string.Empty;
    internal Action? PrimaryCallback { get; set; }
    internal DialogButtonStyle PrimaryButtonStyle { get; set; } = DialogButtonStyle.Primary;
    internal string SecondaryButtonText { get; set; } = string.Empty;
    internal Action? SecondaryCallback { get; set; }
    internal DialogButtonStyle SecondaryButtonStyle { get; set; } = DialogButtonStyle.Secondary;
    internal string TertiaryButtonText { get; set; } = string.Empty;
    internal Action? TertiaryCallback { get; set; }
    internal DialogButtonStyle TertiaryButtonStyle { get; set; } = DialogButtonStyle.Outline;
    internal string CancelButtonText { get; set; } = "Cancel";
    internal DialogButtonStyle CancelButtonStyle { get; set; } = DialogButtonStyle.Outline;
    internal DialogOptions Options { get; } = new();

    internal SimpleDialogBuilder CreateDialog(string title, string message)
    {
        _dialog = new SimpleDialog(_manager)
        {
            Title = title,
            Message = message
        };
        return this;
    }

    internal void Show()
    {
        _dialog ??= new SimpleDialog(_manager);

        _dialog.PrimaryButtonText = PrimaryButtonText;
        _dialog.PrimaryCallback = PrimaryCallback;
        _dialog.PrimaryButtonStyle = PrimaryButtonStyle;
        _dialog.SecondaryButtonText = SecondaryButtonText;
        _dialog.SecondaryCallback = SecondaryCallback;
        _dialog.SecondaryButtonStyle = SecondaryButtonStyle;
        _dialog.TertiaryButtonText = TertiaryButtonText;
        _dialog.TertiaryCallback = TertiaryCallback;
        _dialog.TertiaryButtonStyle = TertiaryButtonStyle;
        _dialog.CancelButtonText = CancelButtonText;
        _dialog.CancelButtonStyle = CancelButtonStyle;

        _manager.Show(_dialog, Options);
    }
}