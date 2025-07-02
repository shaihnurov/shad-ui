using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShadUI.Demo.ViewModels.Examples.Date;

namespace ShadUI.Demo.ViewModels;

public sealed partial class DateViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public DateViewModel(
        IMessenger messenger,
        FormDatePickerViewModel datePickerForm,
        FormDateInputViewModel formDateInput)
    {
        _messenger = messenger;
        DatePickerForm = datePickerForm;
        DateInputForm = formDateInput;

        var xamlPath = Path.Combine(AppContext.BaseDirectory, "views", "DatePage.axaml");
        CalendarCode = xamlPath.ExtractByLineRange(59, 61).CleanIndentation();
        DatePickerCode = xamlPath.ExtractByLineRange(64, 67).CleanIndentation();
        ReadOnlyDatePickerCode = xamlPath.ExtractByLineRange(70, 74).CleanIndentation();
        DateInputCode = xamlPath.ExtractByLineRange(97, 99).CleanIndentation();
        DisabledDateInputCode = xamlPath.ExtractByLineRange(102, 104).CleanIndentation();
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
    private FormDatePickerViewModel _datePickerForm;

    [ObservableProperty]
    private FormDateInputViewModel _dateInputForm;

    [ObservableProperty]
    private string _dateInputCode = string.Empty;

    [ObservableProperty]
    private string _disabledDateInputCode = string.Empty;

    public string Route => "date";
}