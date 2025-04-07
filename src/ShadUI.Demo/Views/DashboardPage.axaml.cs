using Avalonia.Controls;
using Avalonia.Interactivity;
using ShadUI.Demo.ViewModels;
using ShadUI.Themes;

namespace ShadUI.Demo.Views;

public partial class DashboardPage : UserControl
{
    public DashboardPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not DashboardViewModel vm) return;

        vm.ThemeWatcher.ThemeChanged += OnThemeChanged;
    }

    private void OnThemeChanged(object? sender, ThemeColors e)
    {
        CartesianChart1.CoreChart.Update(new LiveChartsCore.Kernel.ChartUpdateParams
            { IsAutomaticUpdate = false, Throttling = false });
        CartesianChart2.CoreChart.Update(new LiveChartsCore.Kernel.ChartUpdateParams
            { IsAutomaticUpdate = false, Throttling = false });
    }
}