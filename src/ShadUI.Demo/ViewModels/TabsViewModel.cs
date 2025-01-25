using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class TabsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _basicTabCode = """
                                   <TabControl>
                                       <TabControl.Items>
                                           <TabItem Header="Preview">
                                               <StackPanel Spacing="8">
                                                   <TextBlock Text="h1" Classes="h4" />
                                                   <Separator Margin="0" />
                                                   <Border Classes="Card Bordered" Padding="48">
                                                       <shadui:Card Padding="48">
                                                           Text="Taxing Laughter: The Joke Tax Chronicles" />
                                                       </shadui:Card>
                                                   </Border>
                                               </StackPanel>
                                           </TabItem>
                                           <TabItem Header="Code">
                                               <controls:CodeTextBlock Height="150" Text="{Binding H1Code}" Language="xaml" />
                                           </TabItem>
                                       </TabControl.Items>
                                   </TabControl>
                                   """;

    [ObservableProperty]
    private string _h1Code = """
                             <TextBlock TextWrapping="Wrap" Classes="h1"
                                        Text="Taxing Laughter: The Joke Tax Chronicles" />
                             """;
}