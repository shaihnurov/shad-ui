using System.Threading.Tasks;
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
    ToggleSwitchViewModel toggleSwitchViewModel,
    SettingsViewModel settingsViewModel)
    : ViewModelBase
{
    [ObservableProperty]
    private object? _selectedPage;

    private async Task<bool> SwitchPage(object page)
    {
        await Task.Delay(200); // prevent flickering

        if (SelectedPage == page) return false;

        SelectedPage = page;
        return true;
    }

    [RelayCommand]
    private async Task OpenDashboard()
    {
        await SwitchPage(dashboardViewModel);
    }

    [RelayCommand]
    private async Task OpenThemes()
    {
        await SwitchPage(themesViewModel);
    }

    [RelayCommand]
    private async Task OpenTypography()
    {
        await SwitchPage(typographyViewModel);
    }

    [RelayCommand]
    private async Task OpenButtons()
    {
        await SwitchPage(buttonsViewModel);
    }

    [RelayCommand]
    private async Task OpenInputs()
    {
        if (await SwitchPage(inputViewModel))
            inputViewModel.Initialize();
    }

    [RelayCommand]
    private async Task OpenTabs()
    {
        await SwitchPage(tabsViewModel);
    }

    [RelayCommand]
    private async Task OpenToggleSwitch()
    {
        await SwitchPage(toggleSwitchViewModel);
    }

    [RelayCommand]
    private async Task OpenSettings()
    {
        await SwitchPage(settingsViewModel);
    }

    public async Task Initialize()
    {
        if (await SwitchPage(dashboardViewModel))
            dashboardViewModel.Initialize();
    }
}