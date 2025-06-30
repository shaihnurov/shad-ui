using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ToastViewModel : ViewModelBase
{
    private readonly ToastManager _toastManager;

    public ToastViewModel(ToastManager toastManager)
    {
        _toastManager = toastManager;
        var path = Path.Combine(AppContext.BaseDirectory, "viewModels", "ToastViewModel.cs");
        SimpleToastCode = WrapCode(path.ExtractByLineRange(40, 44).CleanIndentation());
        WithTitleToastCode = WrapCode(path.ExtractByLineRange(49, 55).CleanIndentation());
        WithActionToastCode = WrapCode(path.ExtractByLineRange(60, 67).CleanIndentation());
        WithDelayToastCode = WrapCode(path.ExtractByLineRange(72, 79).CleanIndentation());
        InfoToastCode = WrapCode(path.ExtractByLineRange(84, 91).CleanIndentation());
        SuccessToastCode = WrapCode(path.ExtractByLineRange(96, 103).CleanIndentation());
        WarningToastCode = WrapCode(path.ExtractByLineRange(108, 115).CleanIndentation());
        ErrorToastCode = WrapCode(path.ExtractByLineRange(120, 127).CleanIndentation());
    }

    private string WrapCode(string code)
    {
        return $"""
                using CommunityToolkit.Mvvm.Input;

                //..other code

                {code}

                //..rest of the code
                """;
    }

    [RelayCommand]
    private void ShowSimpleToast()
    {
        _toastManager.CreateToast("Your message has been sent.").Show();
    }

    [ObservableProperty]
    private string _simpleToastCode = string.Empty;

    [RelayCommand]
    private void ShowToastWithTitle()
    {
        _toastManager.CreateToast("Uh oh! Something went wrong.")
            .WithContent("There was a problem with your request.")
            .Show();
    }

    [ObservableProperty]
    private string _withTitleToastCode = string.Empty;

    [RelayCommand]
    private void ShowToastWithAction()
    {
        _toastManager.CreateToast("Uh oh! Something went wrong.")
            .WithContent("There was a problem with your request.")
            .WithAction("Try again", () => _toastManager.CreateToast("Retry clicked").Show())
            .Show();
    }

    [ObservableProperty]
    private string _withActionToastCode = string.Empty;

    [RelayCommand]
    private void ShowToastWithDelay()
    {
        _toastManager.CreateToast("This toast will disappear in 5 seconds.")
            .WithContent("You can hover over it to prevent it from disappearing.")
            .WithDelay(5)
            .Show();
    }

    [ObservableProperty]
    private string _withDelayToastCode = string.Empty;

    [RelayCommand]
    private void ShowInfoToast()
    {
        _toastManager.CreateToast("Event has been created")
            .WithContent($"{DateTime.Now:dddd, MMMM d 'at' h:mm tt}")
            .DismissOnClick()
            .ShowInfo();
    }

    [ObservableProperty]
    private string _infoToastCode = string.Empty;

    [RelayCommand]
    private void ShowSuccessToast()
    {
        _toastManager.CreateToast("Event has been created successfully")
            .WithContent($"{DateTime.Now:dddd, MMMM d 'at' h:mm tt}")
            .DismissOnClick()
            .ShowSuccess();
    }

    [ObservableProperty]
    private string _successToastCode = string.Empty;

    [RelayCommand]
    private void ShowWarningToast()
    {
        _toastManager.CreateToast("Event has been created with warnings")
            .WithContent($"{DateTime.Now:dddd, MMMM d 'at' h:mm tt}")
            .DismissOnClick()
            .ShowWarning();
    }

    [ObservableProperty]
    private string _warningToastCode = string.Empty;

    [RelayCommand]
    private void ShowErrorToast()
    {
        _toastManager.CreateToast("Failed to create event")
            .WithContent("Unable to connect to the server.")
            .WithAction("Retry", () => _toastManager.CreateToast("Retry clicked").Show())
            .ShowError();
    }

    [ObservableProperty]
    private string _errorToastCode = string.Empty;
}