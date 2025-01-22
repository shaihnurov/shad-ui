using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Dialogs;
using ShadUI.Toasts;

namespace ShadUI.Demo.ViewModels;

public sealed partial class MainWindowViewModel(
    DialogManager dialogManager,
    ToastManager toastManager,
    DashboardViewModel dashboardViewModel,
    ThemesViewModel themesViewModel,
    TypographyViewModel typographyViewModel,
    ButtonsViewModel buttonsViewModel,
    DialogsViewModel dialogsViewModel,
    InputViewModel inputViewModel,
    TabsViewModel tabsViewModel,
    ComboBoxesViewModel comboBoxesViewModel,
    SlidersViewModel slidersViewModel,
    ToastsViewModel toastsViewModel,
    ToggleSwitchViewModel toggleSwitchViewModel,
    SettingsViewModel settingsViewModel)
    : ViewModelBase
{
    [ObservableProperty]
    private DialogManager _dialogManager = dialogManager;
    
    [ObservableProperty]
    private ToastManager _toastManager = toastManager;
    
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
    private async Task OpenDialogs()
    {
        await SwitchPage(dialogsViewModel);
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
    private async Task OpenComboBoxes()
    {
        await SwitchPage(comboBoxesViewModel);
    }

    [RelayCommand]
    private async Task OpenSliders()
    {
        await SwitchPage(slidersViewModel);
    }

    [RelayCommand]
    private async Task OpenToast()
    {
        await SwitchPage(toastsViewModel);
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

    public void Initialize()
    {
        SelectedPage = dashboardViewModel;
        dashboardViewModel.Initialize();
    }
}