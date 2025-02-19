using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Demo.Validators;
using ShadUI.Toasts;

namespace ShadUI.Demo.ViewModels;

public sealed partial class TimeControlViewModel : ViewModelBase
{
    private readonly ToastManager _toastManager;

    public TimeControlViewModel(ToastManager toastManager)
    {
        _toastManager = toastManager;
        PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ValidateAllProperties();
    }

    [ObservableProperty]
    private string _hour12ClockPickerCode = """
                                 <StackPanel>
                                     <TimePicker HorizontalAlignment="Center" />
                                 </StackPanel>
                                 """;

    [ObservableProperty]
    private string _hour24ClockPickerCode = """
                                            <StackPanel>
                                                <TimePicker ClockIdentifier="24HourClock" HorizontalAlignment="Center" UseSeconds="True"/>
                                            </StackPanel>
                                            """;
    [ObservableProperty]
    private string _disabledPickerCode = """
                                   <StackPanel>
                                       <TimePicker IsEnabled="False" HorizontalAlignment="Center" />
                                   </StackPanel>
                                   """;

    [ObservableProperty]
    private string _formPickerCode = """
                               <shadui:Card HorizontalAlignment="Center">
                                   <shadui:Card.Header>
                                       <shadui:CardTitle>Create a schedule</shadui:CardTitle>
                                   </shadui:Card.Header>
                                   <StackPanel Spacing="16">
                                       <TimePicker Width="255"
                                                   extensions:ControlAssist.Label="Start Time"
                                                   extensions:ControlAssist.Hint="Set the beginning time"
                                                   SelectedTime="{Binding StartTime}"/>
                               
                                       <TimePicker Width="255"
                                                   extensions:ControlAssist.Label="End Time"
                                                   extensions:ControlAssist.Hint="Set the cut-off time"
                                                   SelectedTime="{Binding EndTime}"/>
                                   </StackPanel>
                                   <shadui:Card.Footer>
                                       <Button Classes="Primary" Command="{Binding SubmitCommand}">
                                           Submit
                                       </Button>
                                   </shadui:Card.Footer>
                               </shadui:Card>
                               """;

    private TimeSpan? _startTime;

    [Required(ErrorMessage = "Start time is required.")]
    [StartTimeValidation(nameof(EndTime), ErrorMessage = "Start time must be less than end time")]
    public TimeSpan? StartTime
    {
        get => _startTime;
        set => SetProperty(ref _startTime, value, true);
    }

    private TimeSpan? _endTime;
   

    [Required(ErrorMessage = "End time is required.")]
    [EndTimeValidation(nameof(StartTime), ErrorMessage = "End time must be greater than start time")]
    public TimeSpan? EndTime
    {
        get => _endTime;
        set => SetProperty(ref _endTime, value, true);
    }

    [RelayCommand]
    private void Submit()
    {
        ValidateAllProperties();
        if (HasErrors) return;

        _toastManager.CreateToast("Create schedule")
            .WithContent("Schedule created successfully.")
            .DismissOnClick()
            .ShowSuccess();
    }
    
    [ObservableProperty]
    private string _hour12ClockInputCode = """
                                 <StackPanel>
                                     <shadui:TimeInput HorizontalAlignment="Center" />
                                 </StackPanel>
                                 """;

    [ObservableProperty]
    private string _hour24ClockInputCode = """
                                            <StackPanel>
                                                <shadui:TimeInput ClockIdentifier="24HourClock" HorizontalAlignment="Center" UseSeconds="True"/>
                                            </StackPanel>
                                            """;
    [ObservableProperty]
    private string _disabledInputCode = """
                                   <StackPanel>
                                       <shadui:TimeInput IsEnabled="False" HorizontalAlignment="Center" />
                                   </StackPanel>
                                   """;
    [ObservableProperty]
    private string _formInputCode = """
                               <shadui:Card HorizontalAlignment="Center">
                                   <shadui:Card.Header>
                                       <shadui:CardTitle>Create a schedule</shadui:CardTitle>
                                   </shadui:Card.Header>
                                   <StackPanel Spacing="16">
                                       <shadui:TimeInput Width="255"
                                                         extensions:ControlAssist.Label="Start Time"
                                                         extensions:ControlAssist.Hint="Set the beginning time"
                                                         Value="{Binding StartTime, Mode=TwoWay}"/>
                                       <shadui:TimeInput Width="255"
                                                         extensions:ControlAssist.Label="End Time"
                                                         extensions:ControlAssist.Hint="Set the cut-off time"
                                                         Value="{Binding EndTime, Mode=TwoWay}"/>
                                   </StackPanel>
                                   <shadui:Card.Footer>
                                       <Button Classes="Primary" Command="{Binding SubmitCommand}">
                                           Submit
                                       </Button>
                                   </shadui:Card.Footer>
                               </shadui:Card>
                               """;
}