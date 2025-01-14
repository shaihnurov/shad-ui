using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShadUI.Demo.ViewModels;

public sealed partial class MainWindowViewModel(
    DashboardViewModel dashboardViewModel,
    ThemesViewModel themesViewModel,
    TypographyViewModel typographyViewModel,
    ButtonsViewModel buttonsViewModel,
    InputViewModel inputViewModel,
    TabsViewModel tabsViewModel,
    SettingsViewModel settingsViewModel)
    : ViewModelBase
{
    [ObservableProperty]
    private object? _selectedPage;

    private void SwitchPage(object page)
    {
        if (SelectedPage != page) SelectedPage = page;
    }

    [RelayCommand]
    private void OpenDashboard()
    {
        SwitchPage(dashboardViewModel);
    }

    [RelayCommand]
    private void OpenThemes()
    {
        SwitchPage(themesViewModel);
    }

    [RelayCommand]
    private void OpenTypography()
    {
        SwitchPage(typographyViewModel);
    }

    [RelayCommand]
    private void OpenButtons()
    {
        SwitchPage(buttonsViewModel);
    }

    [RelayCommand]
    private void OpenInputs()
    {
        inputViewModel.Initialize();
        SwitchPage(inputViewModel);
    }

    [RelayCommand]
    private void OpenTabs()
    {
        SwitchPage(tabsViewModel);
    }

    [RelayCommand]
    private void OpenSettings()
    {
        SwitchPage(settingsViewModel);
    }

    public void Initialize()
    {
        SwitchPage(dashboardViewModel);
        dashboardViewModel.Initialize();
    }
}