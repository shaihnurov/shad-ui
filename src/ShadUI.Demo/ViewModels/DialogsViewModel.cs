using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Dialogs;
using ShadUI.Toasts;

namespace ShadUI.Demo.ViewModels;

public sealed partial class DialogsViewModel(
    DialogManager dialogManager,
    ToastManager toastManager,
    LoginViewModel loginViewModel
) : ViewModelBase
{
    [ObservableProperty]
    private string _alertDialogCode = """
                                      // Using CommunityToolkit.Mvvm

                                      // ..other code

                                      [RelayCommand]
                                      private void ShowDialog()
                                      {
                                          dialogManager
                                              .CreateDialog(
                                                  "Are you absolutely sure?",
                                                  "This action cannot be undone. This will permanently delete your account and remove your data from our servers.")
                                              .WithPrimaryButton("Continue", OnSubmit)
                                              .WithCancelButton("Cancel")
                                              .WithMaxWidth(512)
                                              .Dismissible()
                                              .Show();
                                      }

                                      // Simulate a long-running operation
                                      private void OnSubmit()
                                      {
                                          toastManager.CreateToast("Delete account")
                                              .WithContent("Account deleted successfully!")
                                              .DismissOnClick()
                                              .ShowSuccess();
                                      }

                                      // ..rest of the code
                                      """;

    [RelayCommand]
    private void ShowDialog()
    {
        dialogManager
            .CreateDialog(
                "Are you absolutely sure?",
                "This action cannot be undone. This will permanently delete your account and remove your data from our servers.")
            .WithPrimaryButton("Continue", OnSubmit)
            .WithCancelButton("Cancel")
            .WithMaxWidth(512)
            .Dismissible()
            .Show();
    }

    private void OnSubmit()
    {
        toastManager.CreateToast("Delete account")
            .WithContent("Account deleted successfully!")
            .DismissOnClick()
            .ShowSuccess();
    }

    [ObservableProperty]
    private string _destructiveAlertDialogCode = """
                                                 // Using CommunityToolkit.Mvvm

                                                 // ..other code

                                                 [RelayCommand]
                                                 private void ShowDialog()
                                                 {
                                                     dialogManager
                                                         .CreateDialog(
                                                             "Are you absolutely sure?",
                                                             "This action cannot be undone. This will permanently delete your account and remove your data from our servers.")
                                                         .WithPrimaryButton("Continue", OnSubmit, DialogButtonStyle.Destructive)
                                                         .WithCancelButton("Cancel")
                                                         .WithMaxWidth(512)
                                                         .Dismissible()
                                                         .Show();
                                                 }

                                                 // Simulate a long-running operation
                                                 private void OnSubmit()
                                                 {
                                                     toastManager.CreateToast("Delete account")
                                                         .WithContent("Account deleted successfully!")
                                                         .DismissOnClick()
                                                         .ShowSuccess();
                                                 }

                                                 // ..rest of the code
                                                 """;

    [RelayCommand]
    private void ShowDestructiveStyleDialog()
    {
        dialogManager
            .CreateDialog(
                "Are you absolutely sure?",
                "This action cannot be undone. This will permanently delete your account and remove your data from our servers.")
            .WithPrimaryButton("Continue", OnSubmit, DialogButtonStyle.Destructive)
            .WithCancelButton("Cancel")
            .WithMaxWidth(512)
            .Dismissible()
            .Show();
    }

    [ObservableProperty]
    private string _customDialogCode = """
                                       // Using CommunityToolkit.Mvvm

                                       // ..other code

                                       [RelayCommand]
                                       private void ShowCustomDialog()
                                       {
                                           loginViewModel.Initialize();
                                           dialogManager.CreateDialog(loginViewModel)
                                               .Dismissible()
                                               .WithSuccessCallback(() =>
                                                   toastManager.CreateToast("Sign in successful")
                                                       .WithContent("Welcome back!")
                                                       .DismissOnClick()
                                                       .ShowSuccess())
                                               .WithCancelCallback(() =>
                                                   toastManager.CreateToast("Sign in cancelled")
                                                       .WithContent("Please sign in to continue.")
                                                       .DismissOnClick()
                                                       .ShowWarning())
                                               .Show();
                                       }

                                       // ..rest of the code
                                       """;

    [RelayCommand]
    private void ShowCustomDialog()
    {
        loginViewModel.Initialize();
        dialogManager.CreateDialog(loginViewModel)
            .Dismissible()
            .WithSuccessCallback(() =>
                toastManager.CreateToast("Sign in successful")
                    .WithContent("Welcome back!")
                    .DismissOnClick()
                    .ShowSuccess())
            .WithCancelCallback(() =>
                toastManager.CreateToast("Sign in cancelled")
                    .WithContent("Please sign in to continue.")
                    .DismissOnClick()
                    .ShowWarning())
            .Show();
    }
}