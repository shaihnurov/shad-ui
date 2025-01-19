using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class SlidersViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _defaultSliderCode = """
                                        <StackPanel>
                                            <Slider Value="5" Maximum="10" TickFrequency="1"/>
                                        </StackPanel>
                                        """;

    [ObservableProperty]
    private string _disabledSliderCode = """
                                         <StackPanel>
                                             <Slider IsEnabled="False" Value="5" Maximum="10" TickFrequency="1" />
                                         </StackPanel>
                                         """;

    [ObservableProperty]
    private string _tickEnabledSliderCode = """
                                            <StackPanel>
                                                <Slider TickPlacement="BottomRight" IsSnapToTickEnabled="True" Value="5" Maximum="10" TickFrequency="1" />
                                            </StackPanel>
                                            """;
}