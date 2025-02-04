using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Toasts;

namespace ShadUI.Demo.ViewModels;

public sealed partial class TimePickerViewModel(ToastManager toastManager) : ViewModelBase
{
    [ObservableProperty]
    private string _simpleCode = """
                                 <StackPanel>
                                     <TimePicker HorizontalAlignment="Center" />
                                 </StackPanel>
                                 """;

    [ObservableProperty]
    private string _disabledCode = """
                                   <StackPanel>
                                       <TimePicker IsEnabled="False" HorizontalAlignment="Center" />
                                   </StackPanel>
                                   """;

    [ObservableProperty]
    private string _formCode = """
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
    public TimeSpan? StartTime
    {
        get => _startTime;
        set => SetProperty(ref _startTime, value, true);
    }

    private TimeSpan? _endTime;

    [Required(ErrorMessage = "End time is required.")]
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

        toastManager.CreateToast("Create schedule")
            .WithContent("Schedule created successfully.")
            .DismissOnClick()
            .ShowSuccess();
    }
}