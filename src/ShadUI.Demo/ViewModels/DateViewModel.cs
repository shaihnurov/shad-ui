using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Demo.Validators;
using ShadUI.Toasts;

namespace ShadUI.Demo.ViewModels;

public sealed partial class DateViewModel : ViewModelBase
{
    private readonly ToastManager _toastManager;

    public DateViewModel(ToastManager toastManager)
    {
        _toastManager = toastManager;

        var path = Path.Combine(AppContext.BaseDirectory, "views", "DatePage.axaml");
        CalendarCode = path.ExtractByLineRange(35, 37).CleanIndentation();
        DatePickerCode = path.ExtractByLineRange(43, 46).CleanIndentation();
        ReadOnlyDatePickerCode = path.ExtractByLineRange(52, 56).CleanIndentation();
        DatePickerFormValidationCode = path.ExtractByLineRange(62, 76).CleanIndentation();
        DateInputCode = path.ExtractByLineRange(82, 84).CleanIndentation();
        DisabledDateInputCode = path.ExtractByLineRange(90, 92).CleanIndentation();
        DateInputFormCode = path.ExtractByLineRange(98, 121).CleanIndentation();
    }

    [ObservableProperty]
    private string _calendarCode = string.Empty;

    [ObservableProperty]
    private string _datePickerCode = string.Empty;

    [ObservableProperty]
    private string _readOnlyDatePickerCode = string.Empty;

    [ObservableProperty]
    private string _datePickerFormValidationCode = string.Empty;

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

        _toastManager.CreateToast("Add birthday")
            .WithContent("Birthday has been submitted.")
            .DismissOnClick()
            .ShowSuccess();
    }

    [ObservableProperty]
    private string _dateInputCode = string.Empty;

    [ObservableProperty]
    private string _disabledDateInputCode = string.Empty;

    private DateOnly? _startDate;

    [Required(ErrorMessage = "Start date is required.")]
    [StartDateValidation(nameof(EndDate), ErrorMessage = "Start date must be less than end date")]
    public DateOnly? StartDate
    {
        get => _startDate;
        set
        {
            SetProperty(ref _startDate, value, true);
            ValidateProperty(EndDate, nameof(EndDate));
        }
    }

    private DateOnly? _endDate;

    [Required(ErrorMessage = "End date is required.")]
    [EndDateValidation(nameof(StartDate), ErrorMessage = "End date must be greater than start date")]
    public DateOnly? EndDate
    {
        get => _endDate;
        set
        {
            SetProperty(ref _endDate, value, true);
            ValidateProperty(StartDate, nameof(StartDate));
        }
    }

    [RelayCommand]
    private void SubmitDateForm()
    {
        ClearAllErrors();

        ValidateProperty(StartDate, nameof(StartDate));
        ValidateProperty(EndDate, nameof(EndDate));
        if (HasErrors) return;

        _toastManager.CreateToast("Create schedule")
            .WithContent("Schedule created successfully.")
            .DismissOnClick()
            .ShowSuccess();
    }

    [ObservableProperty]
    private string _dateInputFormCode = string.Empty;
}