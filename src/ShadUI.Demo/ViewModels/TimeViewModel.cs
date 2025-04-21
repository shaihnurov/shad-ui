using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Demo.Validators;
using ShadUI.Toasts;

namespace ShadUI.Demo.ViewModels;

public sealed partial class TimeViewModel(ToastManager toastManager) : ViewModelBase
{
    [ObservableProperty]
    private string _hour12ClockTimePickerCode = """
                                                <StackPanel>
                                                    <TimePicker HorizontalAlignment="Center" />
                                                </StackPanel>
                                                """;

    [ObservableProperty]
    private string _hour24ClockTimePickerCode = """
                                                <StackPanel>
                                                    <TimePicker ClockIdentifier="24HourClock" HorizontalAlignment="Center" UseSeconds="True"/>
                                                </StackPanel>
                                                """;

    [ObservableProperty]
    private string _disabledTimePickerCode = """
                                             <StackPanel>
                                                 <TimePicker IsEnabled="False" HorizontalAlignment="Center" />
                                             </StackPanel>
                                             """;

    [ObservableProperty]
    private string _formTimePickerCode = """
                                         <shadui:Card HorizontalAlignment="Center">
                                             <shadui:Card.Header>
                                                 <shadui:CardTitle>Create a schedule</shadui:CardTitle>
                                             </shadui:Card.Header>
                                             <StackPanel Spacing="16">
                                                 <TimePicker Width="255"
                                                             extensions:ControlAssist.Label="Start Time"
                                                             extensions:ControlAssist.Hint="Set the beginning time"
                                                             SelectedTime="{Binding StartTime, Converter={x:Static converters:TimeOnlyToTimeSpanConverter.Instance}}" />
                                                 <TimePicker Width="255"
                                                             extensions:ControlAssist.Label="End Time"
                                                             extensions:ControlAssist.Hint="Set the cut-off time"
                                                             SelectedTime="{Binding EndTime, Converter={x:Static converters:TimeOnlyToTimeSpanConverter.Instance}}" />
                                             </StackPanel>
                                             <shadui:Card.Footer>
                                                 <Button Classes="Primary" Command="{Binding SubmitTimeFormCommand}">
                                                     Submit
                                                 </Button>
                                             </shadui:Card.Footer>
                                         </shadui:Card>
                                         """;

    private TimeOnly? _startTime;

    [Required(ErrorMessage = "Start time is required.")]
    [StartTimeValidation(nameof(EndTime), ErrorMessage = "Start time must be less than end time")]
    public TimeOnly? StartTime
    {
        get => _startTime;
        set => SetProperty(ref _startTime, value, true);
    }

    private TimeOnly? _endTime;

    [Required(ErrorMessage = "End time is required.")]
    [EndTimeValidation(nameof(StartTime), ErrorMessage = "End time must be greater than start time")]
    public TimeOnly? EndTime
    {
        get => _endTime;
        set => SetProperty(ref _endTime, value, true);
    }

    [RelayCommand]
    private void SubmitTimeForm()
    {
        ClearAllErrors();

        ValidateProperty(StartTime, nameof(StartTime));
        ValidateProperty(EndTime, nameof(EndTime));
        if (HasErrors) return;

        toastManager.CreateToast("Create schedule")
            .WithContent("Schedule created successfully.")
            .DismissOnClick()
            .ShowSuccess();
    }

    [ObservableProperty]
    private string _hour12ClockTimeInputCode = """
                                               <StackPanel>
                                                   <shadui:TimeInput HorizontalAlignment="Center" />
                                               </StackPanel>
                                               """;

    [ObservableProperty]
    private string _hour24ClockTimeInputCode = """
                                               <StackPanel>
                                                   <shadui:TimeInput ClockIdentifier="24HourClock" HorizontalAlignment="Center" UseSeconds="True"/>
                                               </StackPanel>
                                               """;

    [ObservableProperty]
    private string _disabledTimeInputCode = """
                                            <StackPanel>
                                                <shadui:TimeInput IsEnabled="False" HorizontalAlignment="Center" />
                                            </StackPanel>
                                            """;

    [ObservableProperty]
    private string _formTimeInputCode = """
                                        <shadui:Card HorizontalAlignment="Center">
                                            <shadui:Card.Header>
                                                <shadui:CardTitle>Create a schedule</shadui:CardTitle>
                                            </shadui:Card.Header>
                                            <StackPanel Spacing="16">
                                                <shadui:TimeInput Width="255"
                                                                  extensions:ControlAssist.Label="Start Time"
                                                                  extensions:ControlAssist.Hint="Set the beginning time"
                                                                  Value="{Binding StartTime, Converter={x:Static converters:TimeOnlyToTimeSpanConverter.Instance}}" />
                                                <shadui:TimeInput Width="255"
                                                                  extensions:ControlAssist.Label="End Time"
                                                                  extensions:ControlAssist.Hint="Set the cut-off time"
                                                                  Value="{Binding EndTime, Converter={x:Static converters:TimeOnlyToTimeSpanConverter.Instance}}" />
                                            </StackPanel>
                                            <shadui:Card.Footer>
                                                <Button Classes="Primary" Command="{Binding SubmitTimeFormCommand}">
                                                    Submit
                                                </Button>
                                            </shadui:Card.Footer>
                                        </shadui:Card>
                                        """;
}