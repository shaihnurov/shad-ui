using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Toasts;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ToastsViewModel(ToastManager toastManager) : ViewModelBase
{
    [RelayCommand]
    private void ShowSimpleToast()
    {
        toastManager.CreateToast("Your message has been sent.").Show();
    }

    [ObservableProperty]
    private string _simpleToastCode = """
                                      // Using CommunityToolkit.Mvvm

                                      // ..other code

                                      [RelayCommand]
                                      private void ShowSimpleToast()
                                      {
                                          toastManager.CreateToast("Your message has been sent.").Show();
                                      }
                                      """;

    [RelayCommand]
    private void ShowWithTitleToast()
    {
        toastManager.CreateToast("Uh oh! Something went wrong.")
            .WithContent("There was a problem with your request.")
            .Show();
    }

    [ObservableProperty]
    private string _withTitleToastCode = """
                                         // Using CommunityToolkit.Mvvm

                                         // ..other code

                                         [RelayCommand]
                                         private void ShowWithTitleToast()
                                         {
                                             toastManager.CreateToast("Uh oh! Something went wrong.")
                                                 .WithContent("There was a problem with your request.")
                                                 .Show();
                                         }
                                         """;

    [RelayCommand]
    private void ShowWithActionToast()
    {
        toastManager.CreateToast("Uh oh! Something went wrong.")
            .WithContent("There was a problem with your request.")
            .WithAction("Try again", () => toastManager.CreateToast("Retry clicked").Show())
            .Show();
    }

    [ObservableProperty]
    private string _withActionToastCode = """
                                          // Using CommunityToolkit.Mvvm

                                          // ..other code

                                          [RelayCommand]
                                          private void ShowWithActionToast()
                                          {
                                              toastManager.CreateToast("Uh oh! Something went wrong.")
                                                  .WithContent("There was a problem with your request.")
                                                  .WithAction("Try again", () => toastManager.CreateToast("Retry clicked").Show())
                                                  .Show();
                                          }
                                          """;

    [RelayCommand]
    private void ShowWithDelayToast()
    {
        toastManager.CreateToast("This toast will disappear in 5 seconds.")
            .WithContent("You can hover over it to prevent it from disappearing.")
            .WithDelay(5)
            .Show();
    }

    [ObservableProperty]
    private string _withDelayToastCode = """
                                         // Using CommunityToolkit.Mvvm

                                         // ..other code

                                         [RelayCommand]
                                         private void ShowWithDelayToast()
                                         {
                                             toastManager.CreateToast("This toast will disappear in 5 seconds.")
                                                 .WithContent("You can hover over it to prevent it from disappearing.")
                                                 .WithDelay(5)
                                                 .Show();
                                         }
                                         """;

    [RelayCommand]
    private void ShowInfoToast()
    {
        toastManager.CreateToast("Event has been created")
            .WithContent($"{DateTime.Now:dddd, MMMM d 'at' h:mm tt}")
            .DismissOnClick()
            .ShowInfo();
    }

    [ObservableProperty]
    private string _infoToastCode = """
                                    // Using CommunityToolkit.Mvvm

                                    // ..other code

                                    [RelayCommand]
                                    private void ShowInfoToast()
                                    {
                                        toastManager.CreateToast("Event has been created")
                                            .WithContent($"{DateTime.Now:dddd, MMMM d 'at' h:mm tt}")
                                            .DismissOnClick()
                                            .ShowInfo();
                                    }
                                    """;

    [RelayCommand]
    private void ShowSuccessToast()
    {
        toastManager.CreateToast("Event has been created successfully")
            .WithContent($"{DateTime.Now:dddd, MMMM d 'at' h:mm tt}")
            .DismissOnClick()
            .ShowSuccess();
    }

    [ObservableProperty]
    private string _successToastCode = """
                                       // Using CommunityToolkit.Mvvm

                                       // ..other code

                                       [RelayCommand]
                                       private void ShowSuccessToast()
                                       {
                                           toastManager.CreateToast("Event has been created successfully")
                                               .WithContent($"{DateTime.Now:dddd, MMMM d 'at' h:mm tt}")
                                               .DismissOnClick()
                                               .ShowSuccess();
                                       }
                                       """;

    [RelayCommand]
    private void ShowWarningToast()
    {
        toastManager.CreateToast("Event has been created with warnings")
            .WithContent($"{DateTime.Now:dddd, MMMM d 'at' h:mm tt}")
            .DismissOnClick()
            .ShowWarning();
    }

    [ObservableProperty]
    private string _warningToastCode = """
                                       // Using CommunityToolkit.Mvvm

                                       // ..other code

                                       [RelayCommand]
                                       private void ShowWarningToast()
                                       {
                                           toastManager.CreateToast("Event has been created with warnings")
                                               .WithContent($"{DateTime.Now:dddd, MMMM d 'at' h:mm tt}")
                                               .DismissOnClick()
                                               .ShowWarning();
                                       }
                                       """;

    [RelayCommand]
    private void ShowErrorToast()
    {
        toastManager.CreateToast("Failed to create event")
            .WithContent("Unable to connect to the server.")
            .WithAction("Retry", () => toastManager.CreateToast("Retry clicked").Show())
            .ShowError();
    }

    [ObservableProperty]
    private string _errorToastCode = """
                                     // Using CommunityToolkit.Mvvm

                                     // ..other code

                                     [RelayCommand]
                                     private void ShowErrorToast()
                                     {
                                         toastManager.CreateToast("Failed to create event")
                                             .WithContent("Unable to connect to the server.")
                                             .WithAction("Retry", () => toastManager.CreateToast("Retry clicked").Show())
                                             .ShowError();
                                     }
                                     """;
}