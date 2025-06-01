using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Timers;
using Avalonia.Threading;
using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShadUI.Demo.ViewModels;

public sealed partial class DataGridViewModel : ViewModelBase
{
    private readonly List<DataGridItem> _originalItems =
    [
        new() { Status = Status.Success, Email = "abe45@example.com", Amount = 242 },
        new() { Status = Status.Processing, Email = "monserrat44@example.com", Amount = 837 },
        new() { Status = Status.Success, Email = "silas22@example.com", Amount = 874 },
        new() { Status = Status.Failed, Email = "carmella@example.com", Amount = 721 },
        new() { Status = Status.Success, Email = "ken99@example.com", Amount = 316 }
    ];

    private readonly Timer? _searchTimer;

    public DataGridViewModel()
    {
        _searchTimer = new Timer(500); // 500ms debounce
        _searchTimer.Elapsed += SearchTimerElapsed;
        _searchTimer.AutoReset = false;

        PropertyChanged += OnPropertyChanged;

        foreach (var i in _originalItems) i.PropertyChanged += OnItemsChanged;
        Items = new ObservableCollection<DataGridItem>(_originalItems);
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SearchString))
        {
            if (SearchString.Length > 0)
            {
                IsSearching = true;
                _searchTimer?.Stop();
                _searchTimer?.Start();
            }
            else
            {
                _searchTimer?.Stop();
                IsSearching = false;
                Items.Clear();
                Items.AddRange(_originalItems);
                UpdateTotal();
            }
        }
    }

    private void SearchTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            var filteredItems = _originalItems
                .Where(item => item.Email.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Items.Clear();
            Items.AddRange(filteredItems);

            IsSearching = false;
            _searchTimer?.Stop();
            UpdateTotal();
        });
    }

    private void OnItemsChanged(object? sender, PropertyChangedEventArgs e)
    {
        var selectedAll = Items.All(item => item.IsSelected);
        var notSelectedCount = Items.Count(item => !item.IsSelected);

        if (selectedAll)
        {
            SelectAll = true;
        }
        else if (notSelectedCount == Items.Count)
        {
            SelectAll = false;
        }
        else
        {
            SelectAll = null;
        }

        UpdateTotal();
    }

    private void UpdateTotal()
    {
        TotalCount = Items.Count;
        SelectedCount = Items.Count(item => item.IsSelected);
    }

    [ObservableProperty]
    private string _searchString = string.Empty;

    [ObservableProperty]
    private bool _isSearching;

    [ObservableProperty]
    private bool? _selectAll = false;

    [RelayCommand]
    private void ToggleSelection(bool? selectAll)
    {
        foreach (var item in Items)
        {
            item.IsSelected = selectAll ?? false;
        }
    }

    [ObservableProperty]
    private int _selectedCount;

    [ObservableProperty]
    private int _totalCount;

    [ObservableProperty]
    private ObservableCollection<DataGridItem> _items;

    [ObservableProperty]
    private bool _showStatusColumn = true;

    [RelayCommand]
    private void ToggleStatusColumn()
    {
        ShowStatusColumn = !ShowStatusColumn;
    }

    [ObservableProperty]
    private bool _showEmailColumn = true;

    [RelayCommand]
    private void ToggleEmailColumn()
    {
        ShowEmailColumn = !ShowEmailColumn;
    }

    [ObservableProperty]
    private bool _showAmountColumn = true;

    [RelayCommand]
    private void ToggleAmountColumn()
    {
        ShowAmountColumn = !ShowAmountColumn;
    }

    [ObservableProperty]
    private string _code = """
                           <DataGrid
                               CanUserReorderColumns="False"
                               CanUserSortColumns="True"
                               GridLinesVisibility="Horizontal"
                               ItemsSource="{Binding Items}"
                               SelectionMode="Single"
                               x:Name="DataGrid">
                               <DataGrid.Columns>
                                   <DataGridCheckBoxColumn Binding="{Binding IsSelected}" CanUserSort="False">
                                       <DataGridCheckBoxColumn.Header>
                                           <CheckBox
                                               Command="{Binding ToggleSelectionCommand}"
                                               CommandParameter="{Binding #SelectToggler.IsChecked}"
                                               IsChecked="{Binding SelectAll}"
                                               x:Name="SelectToggler" />
                                       </DataGridCheckBoxColumn.Header>
                                   </DataGridCheckBoxColumn>
                                   <DataGridTextColumn
                                       Binding="{Binding Status}"
                                       CanUserSort="False"
                                       Header="Status"
                                       IsReadOnly="True"
                                       IsVisible="{Binding ShowStatusColumn}" />
                                   <DataGridTextColumn
                                       Binding="{Binding Email, Mode=TwoWay}"
                                       Header="Email"
                                       IsVisible="{Binding ShowEmailColumn}"
                                       Width="*" />
                                   <DataGridTemplateColumn IsVisible="{Binding ShowAmountColumn}" SortMemberPath="Amount">
                                       <DataGridTemplateColumn.Header>
                                           <TextBlock
                                               HorizontalAlignment="Center"
                                               Text="Amount"
                                               TextAlignment="Center" />
                                       </DataGridTemplateColumn.Header>
                                       <DataGridTemplateColumn.CellTemplate>
                                           <DataTemplate DataType="viewModels:DataGridItem">
                                               <TextBlock
                                                   HorizontalAlignment="Right"
                                                   Text="{Binding Amount, StringFormat={}{0:C}}"
                                                   TextAlignment="Right"
                                                   VerticalAlignment="Center" />
                                           </DataTemplate>
                                       </DataGridTemplateColumn.CellTemplate>
                                       <DataGridTemplateColumn.CellEditingTemplate>
                                           <DataTemplate DataType="viewModels:DataGridItem">
                                               <TextBox Text="{Binding Amount}" TextAlignment="End" />
                                           </DataTemplate>
                                       </DataGridTemplateColumn.CellEditingTemplate>
                                   </DataGridTemplateColumn>
                                   <DataGridTemplateColumn CanUserResize="False" CanUserSort="False">
                                       <DataGridTemplateColumn.CellTemplate>
                                           <DataTemplate DataType="viewModels:DataGridItem">
                                               <Menu>
                                                   <MenuItem Classes="Icon Grid" extensions:MenuItemAssist.PopupPlacement="BottomEdgeAlignedRight">
                                                       <extensions:MenuItemAssist.Label>
                                                           <TextBlock Classes="Small" Text="Actions" />
                                                       </extensions:MenuItemAssist.Label>
                                                       <MenuItem.Header>
                                                           <icons:Lucide
                                                               Height="12"
                                                               Icon="Ellipsis"
                                                               StrokeBrush="{DynamicResource ForegroundColor}"
                                                               Width="12" />
                                                       </MenuItem.Header>
                                                       <MenuItem Header="Copy payment ID" />
                                                       <MenuItem Header="View customer" />
                                                       <MenuItem Header="View payment details" />
                                                   </MenuItem>
                                               </Menu>
                                           </DataTemplate>
                                       </DataGridTemplateColumn.CellTemplate>
                                   </DataGridTemplateColumn>
                               </DataGrid.Columns>
                           </DataGrid>
                           """;
}

public sealed partial class DataGridItem : ObservableValidator
{
    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    private Status _status;

    private string _email = string.Empty;

    [EmailAddress]
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value, true);
    }

    [ObservableProperty]
    private decimal _amount;
}

public enum Status
{
    [Display(Name = "Success")]
    Success,

    [Display(Name = "Processing")]
    Processing,

    [Display(Name = "Failed")]
    Failed
}