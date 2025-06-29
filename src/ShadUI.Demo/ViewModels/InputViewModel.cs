using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Timers;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Demo.Validators;

namespace ShadUI.Demo.ViewModels;

public sealed partial class InputViewModel : ViewModelBase
{
    private readonly ToastManager _toastManager;
    private readonly Timer? _searchTimer;

    public InputViewModel(ToastManager toastManager)
    {
        _toastManager = toastManager;
        PropertyChanged += OnPropertyChanged;
        ErrorsChanged += (_, _) => SubmitCommand.NotifyCanExecuteChanged();

        _searchTimer = new Timer(500); // 500ms debounce
        _searchTimer.Elapsed += SearchTimerElapsed;
        _searchTimer.AutoReset = false;

        var path = Path.Combine(AppContext.BaseDirectory, "views", "InputPage.axaml");
        DefaultInputCode = path.ExtractByLineRange(35, 37).CleanIndentation();
        DisabledCode = path.ExtractByLineRange(43, 46).CleanIndentation();
        WithLabelCode = path.ExtractByLineRange(52, 56).CleanIndentation();
        WithCustomLabelCode = path.ExtractByLineRange(62, 66).CleanIndentation();
        SearchBoxCode = path.ExtractByLineRange(72, 84).CleanIndentation();
        AutoCompleteBoxCode = path.ExtractByLineRange(90, 105).CleanIndentation();
        TextAreaCode = path.ExtractByLineRange(111, 116).CleanIndentation();
        FormValidationCode = path.ExtractByLineRange(122, 157).CleanIndentation();
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        SubmitCommand.NotifyCanExecuteChanged();

        if (e.PropertyName == nameof(SearchString))
        {
            if (SearchString.Length > 0)
            {
                IsSearching = true;
                _searchTimer?.Stop();
                _searchTimer?.Start();
            }
            else
            {
                _searchTimer?.Stop();
                IsSearching = false;
            }
        }
    }

    private void SearchTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            IsSearching = false;
            _searchTimer?.Stop();
        });
    }

    [ObservableProperty]
    private string _defaultInputCode = string.Empty;

    [ObservableProperty]
    private string _disabledCode = string.Empty;

    [ObservableProperty]
    private string _withLabelCode = string.Empty;

    [ObservableProperty]
    private string _withCustomLabelCode = string.Empty;

    [ObservableProperty]
    private string _formValidationCode = string.Empty;

    [ObservableProperty]
    private string _searchString = string.Empty;

    [ObservableProperty]
    private bool _isSearching;

    [ObservableProperty]
    private string _searchBoxCode = string.Empty;

    [ObservableProperty]
    private string[] _webFrameworks =
    [
        "Angular",
        "Astro",
        "Lit",
        "Next.js",
        "Nuxt.js",
        "Preact",
        "Qwik",
        "React",
        "Remix",
        "SolidJS",
        "Svelte",
        "SvelteKit",
        "Vue.js"
    ];

    [ObservableProperty]
    private string _autoCompleteBoxCode = string.Empty;

    [ObservableProperty]
    private string _textAreaCode = string.Empty;

    private string _email = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailValidation]
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value, true);
    }

    private string _password = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value, true);
    }

    private string _confirmPassword = string.Empty;

    [Required(ErrorMessage = "Confirm password is required")]
    [IsMatchWith(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value, true);
    }

    [RelayCommand]
    private void Submit()
    {
        ClearAllErrors();
        ValidateAllProperties();

        if (HasErrors) return;

        _toastManager.CreateToast("Form submitted")
            .WithContent("Form submitted successfully!")
            .DismissOnClick()
            .ShowSuccess();

        Initialize();
    }

    public void Initialize()
    {
        Email = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;

        ClearAllErrors();
    }
}