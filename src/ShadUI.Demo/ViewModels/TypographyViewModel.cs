using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class TypographyViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _h1Code = """
                             <TextBlock TextWrapping="Wrap" Classes="h1" 
                                        Text="Taxing Laughter: The Joke Tax Chronicles" />
                             """;

    [ObservableProperty]
    private string _h2Code = """
                             <TextBlock TextWrapping="Wrap" Classes="h2" 
                                        Text="The People of the Kingdom" />
                             """;

    [ObservableProperty]
    private string _h3Code = """
                             <TextBlock TextWrapping="Wrap" Classes="h3" 
                                        Text="The Joke Tax" />
                             """;

    [ObservableProperty]
    private string _h4Code = """
                             <TextBlock TextWrapping="Wrap" Classes="h4" 
                                        Text="People stopped telling jokes" />
                             """;

    [ObservableProperty]
    private string _pCode = """
                            <TextBlock TextWrapping="Wrap" Classes="p"
                                       Text="The king, seeing how much happier his subjects were, realized the error of his ways and repealed the joke tax." />
                            """;

    [ObservableProperty]
    private string _leadCode = """
                               <TextBlock TextWrapping="Wrap" Classes="Lead"
                                          Text="A modal dialog that interrupts the user with important content and expects a response." />
                               """;

    [ObservableProperty]
    private string _largeCode = """
                                <TextBlock TextWrapping="Wrap" TextAlignment="Center" Classes="Large"
                                           Text="Are you absolutely sure?" />
                                """;

    [ObservableProperty]
    private string _smallCode = """
                                <TextBlock TextWrapping="Wrap" Classes="Small" 
                                           Text="Email address" />
                                """;

    [ObservableProperty]
    private string _captionCode = """
                                  <TextBlock TextWrapping="Wrap" TextAlignment="Center" Classes="Caption" 
                                             Text="Username" />
                                  """;

    [ObservableProperty]
    private string _mutedCode = """
                                <TextBlock TextWrapping="Wrap" Classes="Muted" 
                                           Text="Enter your email address" />
                                """;

    [ObservableProperty]
    private string _errorCode = """
                                <TextBlock TextWrapping="Wrap" Classes="Caption Error" 
                                           Text="Invalid email address" />
                                """;
}