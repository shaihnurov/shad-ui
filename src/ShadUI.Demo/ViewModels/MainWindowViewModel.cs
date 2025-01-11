using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShadUI.Demo.ViewModels;

public sealed partial class MainWindowViewModel(
    DashboardViewModel dashboardViewModel,
    ThemesViewModel themesViewModel,
    TypographyViewModel typographyViewModel,
    ButtonsViewModel buttonsViewModel,
    SettingsViewModel settingsViewModel)
    : ViewModelBase
{
    [ObservableProperty]
    private object? _selectedPage;

    [RelayCommand]
    private void OpenDashboard()
    {
        SelectedPage = dashboardViewModel;
    }

    [RelayCommand]
    private void OpenThemes()
    {
        SelectedPage = themesViewModel;
    }

    [RelayCommand]
    private void OpenTypography()
    {
        SelectedPage = typographyViewModel;
    }

    [RelayCommand]
    private void OpenButtons()
    {
        SelectedPage = buttonsViewModel;
    }

    [RelayCommand]
    private void OpenSettings()
    {
        SelectedPage = settingsViewModel;
    }

    public void Initialize()
    {
        SelectedPage = dashboardViewModel;
        dashboardViewModel.Initialize();
    }
}