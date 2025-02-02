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
                                           CornerRadius="{DynamicResource LargeCornerRadius}" Height="400">
                                       <Panel>
                                           <Button Classes="Outline" VerticalAlignment="Center" HorizontalAlignment="Center"
                                                   Command="{Binding ToggleBusyCommand}">
                                               Load
                                           </Button>
                                           <shadui:BusyArea CornerRadius="{DynamicResource LargeCornerRadius}" IsBusy="{Binding IsBusy}" />
                                       </Panel>
                                   </Border>
                                   """;

    [RelayCommand]
    private async Task ToggleBusy()
    {
        IsBusy = true;
        await Task.Delay(3000);
        IsBusy = false;
    }
}