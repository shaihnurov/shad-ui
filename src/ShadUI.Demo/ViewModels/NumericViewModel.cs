using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class NumericViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;
    private readonly ToastManager _toastManager;

    public NumericViewModel(IMessenger messenger, ToastManager toastManager)
    {
        _messenger = messenger;
        _toastManager = toastManager;
        PropertyChanged += OnPropertyChanged;
        ErrorsChanged += (_, _) => SubmitCommand.NotifyCanExecuteChanged();

        var path = Path.Combine(AppContext.BaseDirectory, "views", "NumericPage.axaml");
        DefaultNumericCode = path.ExtractByLineRange(61, 63).CleanIndentation();
        DisabledCode = path.ExtractByLineRange(69, 71).CleanIndentation();
        LeftAlignedCode = path.ExtractByLineRange(77, 79).CleanIndentation();
        WithLabelCode = path.ExtractByLineRange(85, 88).CleanIndentation();
        WithCustomLabelCode = path.ExtractByLineRange(94, 105).CleanIndentation();
        FormValidationCode = path.ExtractByLineRange(111, 134).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(MenuViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(SidebarViewModel) });
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        SubmitCommand.NotifyCanExecuteChanged();
    }

    [ObservableProperty]
    private string _defaultNumericCode = string.Empty;

    [ObservableProperty]
    private string _disabledCode = string.Empty;

    [ObservableProperty]
    private string _leftAlignedCode = string.Empty;

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

    public string Route => "numeric";
}