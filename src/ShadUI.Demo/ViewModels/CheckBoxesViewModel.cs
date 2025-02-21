using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public partial class CheckBoxesViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _defaultCode = """
                                  <StackPanel Spacing="8">
                                      <CheckBox HorizontalAlignment="Center">Accept terms and conditions</CheckBox>
                                  </StackPanel>
                                  """;

    [ObservableProperty]
    private string _disabledCode = """
                                   <StackPanel Spacing="8">
                                       <CheckBox IsEnabled="False" HorizontalAlignment="Center">Accept terms and conditions</CheckBox>
                                   </StackPanel>
                                   """;

    [ObservableProperty]
    private string _indeterminateCode = """
                                        <StackPanel Spacing="8">
                                            <CheckBox IsChecked="{x:Null}" HorizontalAlignment="Center">Accept terms and conditions</CheckBox>
                                        </StackPanel>
                                        """;

    [ObservableProperty]
    private string _multipleCode = """
                                   <StackPanel HorizontalAlignment="Center" Spacing="8">
                                       <StackPanel>
                                           <TextBlock FontSize="16" FontWeight="Medium" Text="Sidebar" />
                                           <TextBlock Classes="Caption Muted"
                                                      Text="Select the items you want to display in the sidebar." />
                                       </StackPanel>
                                       <CheckBox IsChecked="{Binding IsChecked}">Select All</CheckBox>
                                       <ItemsControl Margin="28,0,0,0" ItemsSource="{Binding Items}">
                                           <ItemsControl.ItemTemplate>
                                               <DataTemplate DataType="{x:Type viewModels:CheckBoxItem}">
                                                   <CheckBox Margin="0,2" IsChecked="{Binding IsChecked}"
                                                             Content="{Binding Text}" />
                                               </DataTemplate>
                                           </ItemsControl.ItemTemplate>
                                       </ItemsControl>
                                   </StackPanel>
                                   """;

    public CheckBoxesViewModel()
    {
        Items.CollectionChanged += (_, _) => UpdateSelectAllState();
        foreach (var item in Items) item.PropertyChanged += (_, _) => UpdateSelectAllState();
    }

    partial void OnIsCheckedChanged(bool? value)
    {
        if (value.HasValue)
            foreach (var item in Items)
                item.IsChecked = value.Value;
    }

    private void UpdateSelectAllState()
    {
        IsChecked = Items.All(i => i.IsChecked == true) ? true :
            Items.All(i => i.IsChecked == false) ? false : null;
    }

    [ObservableProperty]
    private bool? _isChecked = false;

    [ObservableProperty]
    private ObservableCollection<CheckBoxItem> _items =
    [
        new() { IsChecked = false, Text = "Recents" },
        new() { IsChecked = false, Text = "Home" },
        new() { IsChecked = false, Text = "Applications" },
        new() { IsChecked = false, Text = "Desktop" },
        new() { IsChecked = false, Text = "Downloads" },
        new() { IsChecked = false, Text = "Documents" }
    ];
}

public partial class CheckBoxItem : ObservableObject
{
    [ObservableProperty]
    private bool? _isChecked;

    [ObservableProperty]
    private string _text = string.Empty;
}