using Avalonia.Controls;
using ShadUI.Demo.ViewModels;
using Window = ShadUI.Controls.Window;

namespace ShadUI.Demo.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Closing += OnClosing;
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;
        
        if(DataContext is MainWindowViewModel viewModel)
        {
            viewModel.TryCloseCommand.Execute(null);
        }
    }
}