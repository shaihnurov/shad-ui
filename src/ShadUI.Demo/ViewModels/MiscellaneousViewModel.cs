using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShadUI.Demo.ViewModels;

public sealed partial class MiscellaneousViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _busyAreaCode = """
                                   <Border BorderThickness="1" BorderBrush="{DynamicResource BorderColor}"
                                           CornerRadius="{DynamicResource LgCornerRadius}" Height="400">
                                       <Panel>
                                           <Button Classes="Outline" VerticalAlignment="Center" HorizontalAlignment="Center"
                                                   Command="{Binding ToggleBusyCommand}">
                                               Load
                                           </Button>
                                           <shadui:BusyArea CornerRadius="{DynamicResource LgCornerRadius}" IsBusy="{Binding IsBusy}" />
                                       </Panel>
                                   </Border>
                                   """;

    [ObservableProperty]
    private string _skeletonCode = """
                                   <StackPanel Orientation="Horizontal" Spacing="16" Margin="24" HorizontalAlignment="Center">
                                       <shadui:Skeleton Height="64" Width="64"
                                                          CornerRadius="{DynamicResource FullCornerRadius}" />
                                       <StackPanel Spacing="8" VerticalAlignment="Center">
                                           <shadui:Skeleton Height="28" Width="255" HorizontalAlignment="Left" />
                                           <shadui:Skeleton Height="20" Width="200" HorizontalAlignment="Left" />
                                       </StackPanel>
                                   </StackPanel>
                                   """;

    [RelayCommand]
    private async Task ToggleBusy()
    {
        IsBusy = true;
        await Task.Delay(3000);
        IsBusy = false;
    }
}