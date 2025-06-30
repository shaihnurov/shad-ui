using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShadUI.Demo.Validators;

namespace ShadUI.Demo.ViewModels;

public sealed partial class DateViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;
    private readonly ToastManager _toastManager;

    public DateViewModel(IMessenger messenger, ToastManager toastManager)
    {
        _messenger = messenger;
        _toastManager = toastManager;

        var path = Path.Combine(AppContext.BaseDirectory, "views", "DatePage.axaml");
        CalendarCode = path.ExtractByLineRange(62, 64).CleanIndentation();
        DatePickerCode = path.ExtractByLineRange(70, 73).CleanIndentation();
        ReadOnlyDatePickerCode = path.ExtractByLineRange(79, 83).CleanIndentation();
        DatePickerFormValidationCode = path.ExtractByLineRange(89, 103).CleanIndentation();
        DateInputCode = path.ExtractByLineRange(109, 111).CleanIndentation();
        DisabledDateInputCode = path.ExtractByLineRange(117, 119).CleanIndentation();
        DateInputFormCode = path.ExtractByLineRange(125, 148).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(DataTableViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(DialogViewModel) });
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

    public string Route => "date";
}