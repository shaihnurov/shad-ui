using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Dialogs;

namespace ShadUI.Demo.ViewModels;

public sealed partial class DialogsViewModel(DialogService dialogService) : ViewModelBase
{
    [ObservableProperty]
    private string _alertDialogCode = """
                                      // Using CommunityToolkit.Mvvm
                                      
                                      // ..other code
                                      
                                      [RelayCommand]
                                      private void ShowDialog()
                                      {
                                          dialogService.Show(new SimpleDialogOptions
                                          {
                                              Title = "Are you absolutely sure?",
                                              Message =
                                                  "This action cannot be undone. This will permanently delete your account and remove your data from our servers.",
                                              PrimaryButtonText = "Continue",
                                              CancelButtonText = "Cancel",
                                              MaxWidth = 485,
                                              AsyncCallback = OnSubmitAsync
                                          });
                                      }
                                      
                                      // Simulate a long-running operation
                                      private static async Task OnSubmitAsync(SimpleDialogAction action)
                                      {
                                          Console.WriteLine("Waiting for 2 seconds...");
                                          await Task.Delay(2000);
                                          Console.WriteLine(action);
                                      }
                                      
                                      // ..rest of the code
                                      """;
    
    [RelayCommand]
    private void ShowDialog()
    {
        dialogService.Show(new SimpleDialogOptions
        {
            Title = "Are you absolutely sure?",
            Message =
                "This action cannot be undone. This will permanently delete your account and remove your data from our servers.",
            PrimaryButtonText = "Continue",
            CancelButtonText = "Cancel",
            MaxWidth = 485,
            AsyncCallback = OnSubmitAsync
        });
    }

    // Simulate a long-running operation
    private static async Task OnSubmitAsync(SimpleDialogAction action)
    {
        Console.WriteLine("Waiting for 2 seconds...");
        await Task.Delay(2000);
        Console.WriteLine(action);
    }

    [ObservableProperty]
    private string _destructiveAlertDialogCode = """
                                      // Using CommunityToolkit.Mvvm

                                      // ..other code

                                      [RelayCommand]
                                      private void ShowDialog()
                                      {
                                          dialogService.Show(new SimpleDialogOptions
                                          {
                                              Title = "Are you absolutely sure?",
                                              Message = "This action cannot be undone. This will permanently delete your account and remove your data from our servers.",
                                              PrimaryButtonText = "Delete",
                                              PrimaryButtonStyle = SimpleDialogButtonStyle.Destructive,
                                              CancelButtonText = "Cancel",
                                              MaxWidth = 485,
                                              AsyncCallback = OnSubmitAsync
                                          });
                                      }

                                      // Simulate a long-running operation
                                      private static async Task OnSubmitAsync(SimpleDialogAction action)
                                      {
                                          Console.WriteLine("Waiting for 2 seconds...");
                                          await Task.Delay(2000);
                                          Console.WriteLine(action);
                                      }

                                      // ..rest of the code
                                      """;
    
    [RelayCommand]
    private void ShowDestructiveStyleDialog()
    {
        dialogService.Show(new SimpleDialogOptions
        {
            Title = "Are you absolutely sure?",
            Message = "This action cannot be undone. This will permanently delete your account and remove your data from our servers.",
            PrimaryButtonText = "Delete",
            PrimaryButtonStyle = SimpleDialogButtonStyle.Destructive,
            CancelButtonText = "Cancel",
            MaxWidth = 485,
            AsyncCallback = OnSubmitAsync
        });
    }
}