using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ShadUI.Controls;

namespace ShadUI.Dialogs;

/// <summary>
/// Dialog service for showing dialogs.
/// </summary>
/// <param name="hostName"></param>
public sealed class DialogService(string hostName = "PART_DialogHost")
{
    private Window? _window;
    private DialogHost? _dialogHost;

    private Window? GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            return desktop.MainWindow as Window;
        return null;
    }

    /// <summary>
    /// Shows a simple dialog with the provided options.
    /// </summary>
    /// <param name="opts">Dialog options</param>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Window"/> or the <see cref="DialogHost"/> is not available.</exception>
    public void Show(SimpleDialogOptions opts)
    {
        _window ??= GetMainWindow();

        if (_window == null)
            throw new InvalidOperationException(
                "Main window could not be found. Make sure the application is properly initialized.");

        var dialog = new SimpleDialog(new SimpleDialogContext(this, opts));
        if (_dialogHost != null)
        {
            _dialogHost.Dialog = dialog;
            _dialogHost.IsDialogOpen = true;
            _window.HasOpenDialog = true;
            return;
        }

        _dialogHost = _window.Hosts.FirstOrDefault(x => x.Name == hostName) as DialogHost;
        if (_dialogHost == null)
            throw new InvalidOperationException(
                "DialogHost could not be found in the main window. Make sure it is properly initialized.");

        _dialogHost.Dialog = dialog;
        _dialogHost.Dismissible = opts.DismissibleDialog;
        _dialogHost.IsDialogOpen = true;
        _window.HasOpenDialog = true;
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    public void Close()
    {
        if (_window == null || _dialogHost == null) return;

        _dialogHost.IsDialogOpen = false;
        _dialogHost.Dialog = null;
        _window.HasOpenDialog = false;
    }
}