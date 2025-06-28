using Avalonia.Controls;
using Avalonia.Interactivity;
using HotAvalonia;
using Window = ShadUI.Controls.Window;

namespace ShadUI.Demo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Closing += OnClosing;
        FullscreenButton.Click += OnFullScreen;
    }
    
    [AvaloniaHotReload]
    // ReSharper disable once UnusedMember.Local
    // Used by Avalonia Hot Reload. Do not remove.
    private void Initialize()
    {
        FullscreenButton.Click -= OnFullScreen;
        FullscreenButton.Click += OnFullScreen;
    }

    private WindowState _cacheWindowState = WindowState.Normal;

    private void OnFullScreen(object? sender, RoutedEventArgs e)
    {
        if (WindowState != WindowState.FullScreen) _cacheWindowState = WindowState;

        if (WindowState == WindowState.FullScreen)
        {
            WindowState = _cacheWindowState;
            ToolTip.SetTip(FullscreenButton, "Fullscreen");
        }
        else
        {
            WindowState = WindowState.FullScreen;
            ToolTip.SetTip(FullscreenButton, "Exit Fullscreen");
        }
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;

        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.TryCloseCommand.Execute(null);
        }
    }
}