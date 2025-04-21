using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Demo.Validators;
using ShadUI.Toasts;

namespace ShadUI.Demo.ViewModels;

public sealed partial class DateViewModel(ToastManager toastManager) : ViewModelBase
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
    private string _datePickerFormValidationCode = """
                                                   <shadui:Card HorizontalAlignment="Center">
                                                       <shadui:Card.Header>
                                                           <shadui:CardTitle>Add Birthday</shadui:CardTitle>
                                                       </shadui:Card.Header>
                                                       <CalendarDatePicker
                                                           SelectedDate="{Binding SelectedDate}"
                                                           Width="240"
                                                           extensions:ControlAssist.Hint="Your date of birth is used to calculate your age."
                                                           extensions:ControlAssist.Label="Date of birth" />
                                                       <shadui:Card.Footer>
                                                           <Button Classes="Primary" Command="{Binding SubmitCommand}">
                                                               Submit
                                                           </Button>
                                                       </shadui:Card.Footer>
                                                   </shadui:Card>
                                                   """;

    private DateTime? _selectedDate;

    [Required(ErrorMessage = "A date of birth is required.")]
    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set => SetProperty(ref _selectedDate, value, true);
    }

    [RelayCommand]
    private void SubmitBirthday()
    {
        ClearAllErrors();

        ValidateProperty(SelectedDate, nameof(SelectedDate));
        if (HasErrors) return;

        toastManager.CreateToast("Add birthday")
            .WithContent("Birthday has been submitted.")
            .DismissOnClick()
            .ShowSuccess();
    }

    [ObservableProperty]
    private string _dateInputCode = """
                                     <StackPanel>
                                        <shadui:DateInput HorizontalAlignment="Center" />
                                    </StackPanel>
                                    """;

    [ObservableProperty]
    private string _disabledDateInputCode = """
                                            <StackPanel>
                                                <shadui:DateInput IsEnabled="False" HorizontalAlignment="Center" />
                                            </StackPanel>
                                            """;

    private DateOnly? _startDate;

    [Required(ErrorMessage = "Start date is required.")]
    [StartDateValidation(nameof(EndDate), ErrorMessage = "Start date must be less than end date")]
    public DateOnly? StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value, true);
    }

    private DateOnly? _endDate;

    [Required(ErrorMessage = "End date is required.")]
    [EndDateValidation(nameof(StartDate), ErrorMessage = "End date must be greater than start date")]
    public DateOnly? EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value, true);
    }

    [RelayCommand]
    private void SubmitDateForm()
    {
        ClearAllErrors();

        ValidateProperty(StartDate, nameof(StartDate));
        ValidateProperty(EndDate, nameof(EndDate));
        if (HasErrors) return;

        toastManager.CreateToast("Create schedule")
            .WithContent("Schedule created successfully.")
            .DismissOnClick()
            .ShowSuccess();
    }

    [ObservableProperty]
    private string _dateInputFormCode = """
                                        <shadui:Card HorizontalAlignment="Center">
                                            <shadui:Card.Header>
                                                <shadui:CardTitle>Create a schedule</shadui:CardTitle>
                                            </shadui:Card.Header>
                                            <StackPanel Spacing="16">
                                                <shadui:DateInput
                                                    Value="{Binding StartDate, Converter={x:Static converters:DateOnlyToDateTimeOffsetConverter.Instance}}"
                                                    Width="255"
                                                    extensions:ControlAssist.Hint="Set the starting date"
                                                    extensions:ControlAssist.Label="Start date" />
                                                <shadui:DateInput
                                                    Value="{Binding EndDate, Converter={x:Static converters:DateOnlyToDateTimeOffsetConverter.Instance}}"
                                                    Width="255"
                                                    extensions:ControlAssist.Hint="Set the cut-off date"
                                                    extensions:ControlAssist.Label="End date" />
                                            </StackPanel>
                                            <shadui:Card.Footer>
                                                <Button Classes="Primary" Command="{Binding SubmitDateFormCommand}">
                                                    Submit
                                                </Button>
                                            </shadui:Card.Footer>
                                        </shadui:Card>
                                        """;
}