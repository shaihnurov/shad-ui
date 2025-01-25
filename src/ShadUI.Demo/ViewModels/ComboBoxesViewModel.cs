using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ComboBoxesViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _selectComboBoxCode = """
                                         <StackPanel>
                                             <ComboBox Width="255"
                                                       HorizontalContentAlignment="Center"
                                                       SelectedIndex="0">
                                                 <ComboBoxItem>Next.js</ComboBoxItem>
                                                 <ComboBoxItem>SvelteKit</ComboBoxItem>
                                                 <ComboBoxItem>Nuxt.js</ComboBoxItem>
                                                 <ComboBoxItem>Remix</ComboBoxItem>
                                                 <ComboBoxItem>Astro</ComboBoxItem>
                                             </ComboBox>
                                         </StackPanel>
                                         """;

    [ObservableProperty]
    private string _selectComboBoxDisabledCode = """
                                                 <StackPanel Spacing="8">
                                                     <ComboBox Width="255"
                                                               IsEnabled="False"
                                                               HorizontalContentAlignment="Center"
                                                               SelectedIndex="0">
                                                         <ComboBoxItem>Next.js</ComboBoxItem>
                                                         <ComboBoxItem>SvelteKit</ComboBoxItem>
                                                         <ComboBoxItem>Nuxt.js</ComboBoxItem>
                                                         <ComboBoxItem>Remix</ComboBoxItem>
                                                         <ComboBoxItem>Astro</ComboBoxItem>
                                                     </ComboBox>
                                                 </StackPanel>
                                                 """;

    [ObservableProperty]
    private string[] _items =
    [
        "Next.js",
        "SvelteKit",
        "Nuxt.js",
        "Remix",
        "Astro"
    ];

    private string? _selectedItem = "Next.js";

    [Required(ErrorMessage = "Please select an item.")]
    public string? SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value, true);
    }

    [RelayCommand]
    private void Clear()
    {
        SelectedItem = null;
    }

    [ObservableProperty]
    private string _formValidationCode = """
                                         <shadui:Card HorizontalAlignment="Center">
                                             <StackPanel Spacing="16">
                                                 <ComboBox Width="255"
                                                           SelectedItem="{Binding SelectedItem, Mode=TwoWay}"
                                                           ItemsSource="{Binding Items}"
                                                           extensions:ComboBox.Label="Select a framework"
                                                           extensions:ComboBox.Hint="Your favorite web framework" />
                                                 <Button HorizontalAlignment="Right" Margin="0,20,0,0" Classes="Outline"
                                                         Command="{Binding ClearCommand}">
                                                     Clear
                                                 </Button>
                                             </StackPanel>
                                         </shadui:Card>
                                         """;
}