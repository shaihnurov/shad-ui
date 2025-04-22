using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class TogglesViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _default = """
                              <ToggleButton Classes="Default" HorizontalAlignment="Center" VerticalAlignment="Center">
                                  <icons:Lucide Icon="Bold" StrokeThickness="1.5" Width="10" Height="16"
                                                StrokeBrush="{DynamicResource ForegroundColor}" />
                              </ToggleButton>
                              """;

    [ObservableProperty]
    private string _outline = """
                              <ToggleButton Classes="Outline" HorizontalAlignment="Center" VerticalAlignment="Center">
                                  <icons:Lucide Icon="Bold" StrokeThickness="1.5" Width="10" Height="16"
                                                StrokeBrush="{DynamicResource ForegroundColor}" />
                              </ToggleButton>
                              """;

    [ObservableProperty]
    private string _withText = """
                               <ToggleButton Classes="Default" HorizontalAlignment="Center" VerticalAlignment="Center">
                                   <StackPanel Orientation="Horizontal" Spacing="8">
                                       <icons:Lucide Icon="Italic" StrokeThickness="1.5" Width="10" Height="16"
                                                     StrokeBrush="{DynamicResource ForegroundColor}" />
                                       <TextBlock Text="Italic" />
                                   </StackPanel>
                               </ToggleButton>
                               """;

    [ObservableProperty]
    private string _disable = """
                              <ToggleButton Classes="Default" HorizontalAlignment="Center" VerticalAlignment="Center"
                                            IsEnabled="False">
                                  <icons:Lucide Icon="Bold" StrokeThickness="1.5" Width="10" Height="16"
                                                StrokeBrush="{DynamicResource ForegroundColor}" />
                              </ToggleButton>
                              """;
}