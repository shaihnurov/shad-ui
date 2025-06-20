using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Toasts;

namespace ShadUI.Demo.ViewModels;

public sealed partial class NumericViewModel : ViewModelBase
{
    private readonly ToastManager _toastManager;

    public NumericViewModel(ToastManager toastManager)
    {
        _toastManager = toastManager;
        PropertyChanged += OnPropertyChanged;
        ErrorsChanged += (_, _) => SubmitCommand.NotifyCanExecuteChanged();

        var path = Path.Combine(AppContext.BaseDirectory, "views", "NumericPage.axaml");
        DefaultNumericCode = path.ExtractByLineRange(35, 37).CleanIndentation();
        DisabledCode = path.ExtractByLineRange(43, 46).CleanIndentation();
        WithLabelCode = path.ExtractByLineRange(52, 56).CleanIndentation();
        WithCustomLabelCode = path.ExtractByLineRange(62, 66).CleanIndentation();
        FormValidationCode = path.ExtractByLineRange(72, 88).CleanIndentation();
    }

    private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        SubmitCommand.NotifyCanExecuteChanged();
    }

    [ObservableProperty]
    private string _defaultNumericCode = string.Empty;

    [ObservableProperty]
    private string _disabledCode = string.Empty;

    [ObservableProperty]
    private string _withLabelCode = string.Empty;

    [ObservableProperty]
    private string _withCustomLabelCode = string.Empty;

    [ObservableProperty]
    private string _formValidationCode = string.Empty;

    private double? _age;

    [Required(ErrorMessage = "Age is required")]
    [Range(18d, 100d, ErrorMessage = "Age must be between 18 and 100")]
    public double? Age
    {
        get => _age;
        set => SetProperty(ref _age, value, true);
    }

    [RelayCommand]
    private void Submit()
    {
        ValidateAllProperties();

        if (HasErrors) return;

        _toastManager.CreateToast("Form submitted")
            .WithContent("Form submitted successfully!")
            .DismissOnClick()
            .ShowSuccess();
    }

    public void Initialize()
    {
        Age = 0;
        ClearAllErrors();
    }
}