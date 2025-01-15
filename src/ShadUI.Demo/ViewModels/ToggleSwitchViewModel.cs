using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ToggleSwitchViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _enableCode = """
                                       <StackPanel HorizontalAlignment="Center" Orientation="Horizontal" Spacing="8">
                                           <ToggleSwitch>
                                               <ToggleSwitch.OffContent>
                                                   <TextBlock Classes="Small" Text="Airplane Mode Off" VerticalAlignment="Center" />
                                               </ToggleSwitch.OffContent>
                                               <ToggleSwitch.OnContent>
                                                   <TextBlock Classes="Small" Text="Airplane Mode On" VerticalAlignment="Center" />
                                               </ToggleSwitch.OnContent>
                                           </ToggleSwitch>
                                       </StackPanel>
                                       """;

    [ObservableProperty]
    private string _disableCode = """
                                 <StackPanel HorizontalAlignment="Center" Orientation="Horizontal" Spacing="8">
                                     <ToggleSwitch IsEnabled="False" IsChecked="True">
                                         <ToggleSwitch.OffContent>
                                             <TextBlock Classes="Small" Text="Airplane Mode Off" VerticalAlignment="Center" />
                                         </ToggleSwitch.OffContent>
                                         <ToggleSwitch.OnContent>
                                             <TextBlock Classes="Small" Text="Airplane Mode On" VerticalAlignment="Center" />
                                         </ToggleSwitch.OnContent>
                                     </ToggleSwitch>
                                 </StackPanel>
                                 """;
    
    [ObservableProperty]
    private string _rightAlignedCode ="""
                                      <StackPanel HorizontalAlignment="Center" Orientation="Horizontal" Spacing="8">
                                          <ToggleSwitch extensions:ToggleSwitchExt.RightAlignedContent="True">
                                              <ToggleSwitch.OffContent>
                                                  <TextBlock Classes="Small" Text="Airplane Mode Off" VerticalAlignment="Center" />
                                              </ToggleSwitch.OffContent>
                                              <ToggleSwitch.OnContent>
                                                  <TextBlock Classes="Small" Text="Airplane Mode On" VerticalAlignment="Center" />
                                              </ToggleSwitch.OnContent>
                                          </ToggleSwitch>
                                      </StackPanel>
                                      """;
}