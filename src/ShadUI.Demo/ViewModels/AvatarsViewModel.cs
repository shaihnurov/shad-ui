using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class AvatarsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _usageCode = """
                                <StackPanel>
                                    <shadui:Avatar Width="80" Height="80" Source="../../Assets/user.png" />
                                </StackPanel>
                                """;
}