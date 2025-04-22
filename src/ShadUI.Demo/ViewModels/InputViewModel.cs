using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Timers;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Demo.Validators;
using ShadUI.Toasts;

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
    private string _defaultInputCode = """
                                       <StackPanel>
                                           <TextBox Width="225" Watermark="Insert here..." />
                                       </StackPanel>
                                       """;

    [ObservableProperty]
    private string _disabledCode = """
                                   <StackPanel>
                                       <TextBox Width="225" IsEnabled="False" Watermark="Email" />
                                   </StackPanel>
                                   """;

    [ObservableProperty]
    private string _withLabelCode = """
                                    <StackPanel>
                                        <TextBox Classes="Clearable" Width="225" UseFloatingWatermark="True" Watermark="Email" />
                                    </StackPanel>
                                    """;

    [ObservableProperty]
    private string _withCustomLabelCode = """
                                          <StackPanel>
                                              <TextBox Classes="Clearable" Width="225" extensions:ControlAssist.Label="Email"
                                                       Watermark="user@example.com" />
                                          </StackPanel>
                                          """;

    [ObservableProperty]
    private string _formValidationCode = """
                                         <shadui:Card HorizontalAlignment="Center" Width="350">
                                             <shadui:Card.Header>
                                                 <StackPanel Spacing="4">
                                                     <shadui:CardTitle FontSize="18">Creat new account</shadui:CardTitle>
                                                     <shadui:CardDescription>Enter your account details</shadui:CardDescription>
                                                 </StackPanel>
                                             </shadui:Card.Header>
                                             <StackPanel Spacing="16">
                                                 <TextBox Classes="Clearable"
                                                          extensions:ControlAssist.Label="Email"
                                                          extensions:ControlAssist.Hint="This is your public display name."
                                                          Watermark="user@example.com"
                                                          Text="{Binding Email, Mode=TwoWay}" />
                                                 <TextBox Classes="PasswordReveal"
                                                          extensions:ControlAssist.Label="Password"
                                                          PasswordChar="•"
                                                          Watermark="Enter password"
                                                          Text="{Binding Password, Mode=TwoWay}" />
                                                 <TextBox Classes="PasswordReveal"
                                                          PasswordChar="•"
                                                          extensions:ControlAssist.Label="Confirm"
                                                          Watermark="Confirm password"
                                                          Text="{Binding ConfirmPassword, Mode=TwoWay}" />
                                             </StackPanel>
                                             <shadui:Card.Footer>
                                                 <Button Classes="Primary" Command="{Binding SubmitCommand}">Submit</Button>
                                             </shadui:Card.Footer>
                                         </shadui:Card>
                                         """;

    [ObservableProperty]
    private string _searchString = string.Empty;

    [ObservableProperty]
    private bool _isSearching;

    [ObservableProperty]
    private string _searchBoxCode = """
                                    <StackPanel>
                                        <TextBox Classes="Clearable" Width="225"
                                                 Text="{Binding SearchString, Mode=TwoWay}"
                                                 extensions:ControlAssist.ShowProgress="{Binding IsSearching}"
                                                 Watermark="Search here...">
                                            <TextBox.InnerRightContent>
                                                <PathIcon Data="{x:Static contents:Icons.Search}" Opacity="0.75" Width="16" />
                                            </TextBox.InnerRightContent>
                                        </TextBox>
                                    </StackPanel>
                                    """;
    
    [ObservableProperty]
    private string[] _webFrameworks = ["Next.js", "Sveltekit", "Nuxt.js", "Remix", "Astro"];

    [ObservableProperty]
    private string _autoCompleteBoxCode = """
                                          <StackPanel>
                                              <AutoCompleteBox
                                                  FilterMode="Contains"
                                                  ItemsSource="{Binding WebFrameworks}"
                                                  Watermark="Search here..."
                                                  Width="225"
                                                  extensions:ControlAssist.Hint="Your favorite web framework"
                                                  extensions:ControlAssist.Label="Search a framework"
                                                  extensions:ElementAssist.Classes="Clearable">
                                                  <AutoCompleteBox.InnerRightContent>
                                                      <PathIcon
                                                          Data="{x:Static contents:Icons.Search}"
                                                          Opacity="0.75"
                                                          Width="16" />
                                                  </AutoCompleteBox.InnerRightContent>
                                              </AutoCompleteBox>
                                          </StackPanel>
                                          """;

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

    [RelayCommand(CanExecute = nameof(CanSubmit))]
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

    private bool CanSubmit() => !HasErrors;

    public void Initialize()
    {
        Email = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;

        ClearAllErrors();
    }
}