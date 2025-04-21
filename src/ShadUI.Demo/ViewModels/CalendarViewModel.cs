using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Toasts;

namespace ShadUI.Demo.ViewModels;

public sealed partial class CalendarViewModel(ToastManager toastManager) : ViewModelBase
{
    [ObservableProperty]
    private string _defaultCode = """
                                  <Calendar 
                                      HorizontalAlignment="Center"
                                      VerticalAlignment="Center" />
                                  """;

    [ObservableProperty]
    private string _datePickerCode = """
                                     <CalendarDatePicker
                                         HorizontalAlignment="Center"
                                         VerticalAlignment="Center"
                                         Width="240" />
                                     """;

    [ObservableProperty]
    private string _readOnlyDatePickerCode = """
                                             <CalendarDatePicker
                                                 HorizontalAlignment="Center"
                                                 VerticalAlignment="Center"
                                                 Width="240"
                                                 extensions:ControlAssist.ReadOnly="True" />
                                             """;

    [ObservableProperty]
    private string _formValidationCode = """
                                         <StackPanel HorizontalAlignment="Center" VerticalAlignment="Center">
                                             <CalendarDatePicker
                                                 SelectedDate="{Binding SelectedDate}"
                                                 Width="240"
                                                 extensions:ControlAssist.Hint="Your date of birth is used to calculate your age."
                                                 extensions:ControlAssist.Label="Date of birth" />
                                             <Button
                                                 Classes="Primary"
                                                 Command="{Binding SubmitCommand}"
                                                 HorizontalAlignment="Left"
                                                 Margin="0,36,0,0">
                                                 Submit
                                             </Button>
                                         </StackPanel>
                                         """;

    private DateTime? _selectedDate;

    [Required(ErrorMessage = "A date of birth is required.")]
    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set => SetProperty(ref _selectedDate, value, true);
    }

    [RelayCommand]
    private void Submit()
    {
        ValidateAllProperties();
        if (HasErrors) return;

        toastManager.CreateToast("Submit")
            .WithContent("Form has been submitted")
            .DismissOnClick()
            .ShowSuccess();
    }
}