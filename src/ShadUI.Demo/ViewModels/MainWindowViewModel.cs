using System.Diagnostics;
using System.Runtime.InteropServices;
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
    TypographyViewModel typographyViewModel,
    AvatarsViewModel avatarsViewModel,
    ButtonsViewModel buttonsViewModel,
    CardsViewModel cardsViewModel,
    DialogsViewModel dialogsViewModel,
    InputViewModel inputViewModel,
    TabsViewModel tabsViewModel,
    ComboBoxesViewModel comboBoxesViewModel,
    SlidersViewModel slidersViewModel,
    ToastsViewModel toastsViewModel,
    ToggleSwitchViewModel toggleSwitchViewModel,
    ToolTipViewModel toolTipViewModel,
    MiscellaneousViewModel miscellaneousViewModel)
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
        dashboardViewModel.Initialize();
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
    private async Task OpenAvatar()
    {
        await SwitchPage(avatarsViewModel);
    }

    [RelayCommand]
    private async Task OpenCards()
    {
        await SwitchPage(cardsViewModel);
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
    private async Task OpenToggleSwitch()
    {
        await SwitchPage(toggleSwitchViewModel);
    }
    
    [RelayCommand]
    private async Task OpenToast()
    {
        await SwitchPage(toastsViewModel);
    }

    [RelayCommand]
    private async Task OpenToolTip()
    {
        await SwitchPage(toolTipViewModel);
    }

    [RelayCommand]
    private async Task OpenMiscellaneous()
    {
        await SwitchPage(miscellaneousViewModel);
    }
    
    [RelayCommand]
    private void OpenUrl(string url)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            Process.Start(new ProcessStartInfo(url.Replace("&", "^&")) { UseShellExecute = true });
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            Process.Start("xdg-open", url);
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            Process.Start("open", url);
    }
    
    public void Initialize()
    {
        SelectedPage = dashboardViewModel;
        dashboardViewModel.Initialize();
    }
}