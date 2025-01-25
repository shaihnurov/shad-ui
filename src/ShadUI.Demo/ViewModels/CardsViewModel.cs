using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class CardsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _usageCode ="""
                               <shadui:Card Margin="4" HasShadow="True" HorizontalAlignment="Center">
                                   <shadui:Card.Header>
                                       <StackPanel>
                                           <shadui:CardTitle>Create project</shadui:CardTitle>
                                           <shadui:CardDescription>Deploy your new project in one-click.</shadui:CardDescription>
                                       </StackPanel>
                                   </shadui:Card.Header>
                                   <StackPanel Spacing="16">
                                       <TextBox extensions:TextBox.Label="Name" Watermark="Name of your project" />
                                       <ComboBox Width="300"
                                                 SelectedIndex="0"
                                                 extensions:ComboBox.Label="Framework">
                                           <ComboBox.Items>
                                               <ComboBoxItem>Next.js</ComboBoxItem>
                                               <ComboBoxItem>React</ComboBoxItem>
                                               <ComboBoxItem>Vue.js</ComboBoxItem>
                                               <ComboBoxItem>Angular</ComboBoxItem>
                                           </ComboBox.Items>
                                       </ComboBox>
                                   </StackPanel>
                                   <shadui:Card.Footer>
                                       <DockPanel>
                                           <Button HorizontalAlignment="Left" Margin="0,20,0,0" Classes="Outline">
                                               Cancel
                                           </Button>
                                           <Button HorizontalAlignment="Right" Margin="0,20,0,0" Classes="Primary">
                                               Deploy
                                           </Button>
                                       </DockPanel>
                                   </shadui:Card.Footer>
                               </shadui:Card>
                               """;
}