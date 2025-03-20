using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ButtonsViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isBusy;

    [RelayCommand]
    private async Task SubmitAsync()
    {
        IsBusy = true;
        await Task.Delay(5000);
        IsBusy = false;
    }

    [ObservableProperty]
    private string _primaryCode = """
                                  <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                      <Button Classes="Primary" Content="Primary"
                                              extensions:Button.ShowProgress="{Binding IsBusy}" Command="{Binding SubmitCommand}" />
                                      <Button Classes="Primary" Content="Edit" extensions:Button.ShowProgress="{Binding IsBusy}"
                                              Command="{Binding SubmitCommand}">
                                          <extensions:Button.Icon>
                                              <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="10" Height="16"
                                                            StrokeBrush="{DynamicResource PrimaryForegroundColor}" />
                                          </extensions:Button.Icon>
                                      </Button>
                                      <Button Classes="Primary" IsEnabled="False" Content="Disable" />
                                  </StackPanel>
                                  """;

    [ObservableProperty]
    private string _secondaryCode = """
                                    <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                        <Button Classes="Secondary" Content="Secondary"
                                                extensions:Button.ShowProgress="{Binding IsBusy}" Command="{Binding SubmitCommand}" />
                                        <Button Classes="Secondary" Content="Edit"
                                                extensions:Button.ShowProgress="{Binding IsBusy}" Command="{Binding SubmitCommand}">
                                            <extensions:Button.Icon>
                                                <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="10" Height="16"
                                                              StrokeBrush="{DynamicResource SecondaryForegroundColor}" />
                                            </extensions:Button.Icon>
                                        </Button>
                                        <Button Classes="Secondary" IsEnabled="False" Content="Disable" />
                                    </StackPanel>
                                    """;

    [ObservableProperty]
    private string _destructiveCode = """
                                      <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                          <Button Classes="Destructive" Content="Destructive"
                                                  extensions:Button.ShowProgress="{Binding IsBusy}" Command="{Binding SubmitCommand}" />
                                          <Button Classes="Destructive" Content="Delete"
                                                  extensions:Button.ShowProgress="{Binding IsBusy}" Command="{Binding SubmitCommand}">
                                              <extensions:Button.Icon>
                                                  <icons:Lucide Icon="Trash" StrokeThickness="1.5" Width="10" Height="16"
                                                                StrokeBrush="{DynamicResource DestructiveForegroundColor}" />
                                              </extensions:Button.Icon>
                                          </Button>
                                          <Button Classes="Destructive" IsEnabled="False" Content="Disable" />
                                      </StackPanel>
                                      """;

    [ObservableProperty]
    private string _outlineCode = """
                                  <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                      <Button Classes="Outline" Content="Outline"
                                              extensions:Button.ShowProgress="{Binding IsBusy}" Command="{Binding SubmitCommand}" />
                                      <Button Classes="Outline" Content="Edit" extensions:Button.ShowProgress="{Binding IsBusy}"
                                              Command="{Binding SubmitCommand}">
                                          <extensions:Button.Icon>
                                              <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="10" Height="16"
                                                            StrokeBrush="{DynamicResource ForegroundColor}" />
                                          </extensions:Button.Icon>
                                      </Button>
                                      <Button Classes="Outline" IsEnabled="False" Content="Disable" />
                                  </StackPanel>
                                  """;

    [ObservableProperty]
    private string _ghostCode = """
                                <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                    <Button Classes="Ghost" Content="Ghost" extensions:Button.ShowProgress="{Binding IsBusy}"
                                            Command="{Binding SubmitCommand}" />
                                    <Button Classes="Ghost" Content="Edit" extensions:Button.ShowProgress="{Binding IsBusy}"
                                            Command="{Binding SubmitCommand}">
                                        <extensions:Button.Icon>
                                            <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="10" Height="16"
                                                          StrokeBrush="{DynamicResource ForegroundColor}" />
                                        </extensions:Button.Icon>
                                    </Button>
                                    <Button Classes="Ghost" IsEnabled="False" Content="Disable" />
                                </StackPanel>
                                """;

    [ObservableProperty]
    private string _iconCode = """
                               <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                   <Button Classes="Icon" BorderBrush="{DynamicResource BorderColor}"
                                           extensions:Button.ShowProgress="{Binding IsBusy}" Command="{Binding SubmitCommand}">
                                       <extensions:Button.Icon>
                                           <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="16"
                                                         StrokeBrush="{DynamicResource ForegroundColor}" />
                                       </extensions:Button.Icon>
                                   </Button>
                                   <Button Classes="Icon" IsEnabled="False" BorderBrush="{DynamicResource BorderColor}">
                                       <extensions:Button.Icon>
                                           <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="16"
                                                         StrokeBrush="{DynamicResource ForegroundColor}" />
                                       </extensions:Button.Icon>
                                   </Button>
                               </StackPanel>
                               """;

    [ObservableProperty]
    private string _destructiveIconCode = """
                                          <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                              <Button Classes="DestructiveIcon" extensions:Button.ShowProgress="{Binding IsBusy}"
                                                      Command="{Binding SubmitCommand}">
                                                  <extensions:Button.Icon>
                                                      <icons:Lucide Icon="Trash" StrokeThickness="1.5" Width="16"
                                                                    StrokeBrush="{DynamicResource DestructiveForegroundColor}" />
                                                  </extensions:Button.Icon>
                                              </Button>
                                              <Button Classes="DestructiveIcon" IsEnabled="False">
                                                  <extensions:Button.Icon>
                                                      <icons:Lucide Icon="Trash" StrokeThickness="1.5" Width="16"
                                                                    StrokeBrush="{DynamicResource DestructiveForegroundColor}" />
                                                  </extensions:Button.Icon>
                                              </Button>
                                          </StackPanel>
                                          """;
}