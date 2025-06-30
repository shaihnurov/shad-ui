using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Demo.ViewModels;

namespace ShadUI.Demo;

public sealed partial class MainWindowViewModel(
    DialogManager dialogManager,
    ToastManager toastManager,
    ThemeWatcher themeWatcher,
    AboutViewModel aboutViewModel,
    DashboardViewModel dashboardViewModel,
    ThemeViewModel themeViewModel,
    TypographyViewModel typographyViewModel,
    AvatarViewModel avatarViewModel,
    ButtonViewModel buttonViewModel,
    CardViewModel cardViewModel,
    DataGridViewModel dataGridViewModel,
    DateViewModel dateViewModel,
    CheckBoxViewModel checkBoxViewModel,
    DialogViewModel dialogViewModel,
    TimeViewModel timeViewModel,
    InputViewModel inputViewModel,
    NumericViewModel numericViewModel,
    MenuViewModel menuViewModel,
    TabControlViewModel tabControlViewModel,
    ColorViewModel colorViewModel,
    ComboBoxViewModel comboBoxViewModel,
    SidebarViewModel sidebarViewModel,
    SliderViewModel sliderViewModel,
    SwitchViewModel switchViewModel,
    ToastViewModel toastViewModel,
    ToggleViewModel toggleViewModel,
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

    private bool SwitchPage(object page)
    {
        if (SelectedPage == page) return false;

        SelectedPage = page;
        return true;
    }

    [RelayCommand]
    private void OpenDashboard()
    {
        if (SwitchPage(dashboardViewModel))
        {
            dashboardViewModel.Initialize();
        }
    }

    [RelayCommand]
    private void OpenTheme()
    {
        SwitchPage(themeViewModel);
    }

    [RelayCommand]
    private void OpenTypography()
    {
        SwitchPage(typographyViewModel);
    }

    [RelayCommand]
    private void OpenButtons()
    {
        SwitchPage(buttonViewModel);
    }

    [RelayCommand]
    private void OpenAvatar()
    {
        SwitchPage(avatarViewModel);
    }

    [RelayCommand]
    private void OpenCards()
    {
        SwitchPage(cardViewModel);
    }

    [RelayCommand]
    private void OpenDataGrid()
    {
        SwitchPage(dataGridViewModel);
    }

    [RelayCommand]
    private void OpenDate()
    {
        SwitchPage(dateViewModel);
    }

    [RelayCommand]
    private void OpenCheckBoxes()
    {
        SwitchPage(checkBoxViewModel);
    }

    [RelayCommand]
    private void OpenDialogs()
    {
        SwitchPage(dialogViewModel);
    }

    [RelayCommand]
    private void OpenInputs()
    {
        if (SwitchPage(inputViewModel))
        {
            inputViewModel.Initialize();
        }
    }

    [RelayCommand]
    private void OpenNumerics()
    {
        if (SwitchPage(numericViewModel))
        {
            numericViewModel.Initialize();
        }
    }

    [RelayCommand]
    private void OpenMenus()
    {
        SwitchPage(menuViewModel);
    }

    [RelayCommand]
    private void OpenTabs()
    {
        SwitchPage(tabControlViewModel);
    }

    [RelayCommand]
    private void OpenComboBoxes()
    {
        SwitchPage(comboBoxViewModel);
    }

    [RelayCommand]
    private void OpenColors()
    {
        SwitchPage(colorViewModel);
    }

    [RelayCommand]
    private void OpenSidebar()
    {
        SwitchPage(sidebarViewModel);
    }

    [RelayCommand]
    private void OpenSliders()
    {
        SwitchPage(sliderViewModel);
    }

    [RelayCommand]
    private void OpenSwitch()
    {
        SwitchPage(switchViewModel);
    }

    [RelayCommand]
    private void OpenTime()
    {
        SwitchPage(timeViewModel);
    }

    [RelayCommand]
    private void OpenToast()
    {
        SwitchPage(toastViewModel);
    }

    [RelayCommand]
    private void OpenToggle()
    {
        SwitchPage(toggleViewModel);
    }

    [RelayCommand]
    private void OpenToolTip()
    {
        SwitchPage(toolTipViewModel);
    }

    [RelayCommand]
    private void OpenMiscellaneous()
    {
        SwitchPage(miscellaneousViewModel);
    }

    [RelayCommand]
    private void OpenUrl(string url)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start(new ProcessStartInfo(url.Replace("&", "^&")) { UseShellExecute = true });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
    }

    public void Initialize()
    {
        SelectedPage = dashboardViewModel;
        dashboardViewModel.Initialize();
    }

    [RelayCommand]
    private void ShowAbout()
    {
        DialogManager.CreateDialog(aboutViewModel)
            .WithMinWidth(300)
            .Dismissible()
            .Show();
    }

    [RelayCommand]
    private void TryClose()
    {
        DialogManager.CreateDialog("Close", "Do you really want to exit?")
            .WithPrimaryButton("Yes", OnAcceptExit)
            .WithCancelButton("No")
            .WithMinWidth(300)
            .Show();
    }

    private void OnAcceptExit()
    {
        Environment.Exit(0);
    }

    private ThemeMode _currentTheme;

    public ThemeMode CurrentTheme
    {
        get => _currentTheme;
        private set => SetProperty(ref _currentTheme, value);
    }

    [RelayCommand]
    private void SwitchTheme()
    {
        CurrentTheme = CurrentTheme switch
        {
            ThemeMode.System => ThemeMode.Light,
            ThemeMode.Light => ThemeMode.Dark,
            _ => ThemeMode.System
        };

        themeWatcher.SwitchTheme(CurrentTheme);
    }
}