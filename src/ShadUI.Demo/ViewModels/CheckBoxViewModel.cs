using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public partial class CheckBoxViewModel : ViewModelBase
{
    public CheckBoxViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "CheckBoxPage.axaml");
        DefaultCode = path.ExtractByLineRange(33, 35).CleanIndentation();
        DisabledCode = path.ExtractByLineRange(41, 43).CleanIndentation();
        IndeterminateCode = path.ExtractByLineRange(49, 51).CleanIndentation();
        MultipleCode = path.ExtractByLineRange(57, 76).CleanIndentation();

        Items.CollectionChanged += (_, _) => UpdateSelectAllState();
        foreach (var item in Items) item.PropertyChanged += (_, _) => UpdateSelectAllState();
    }

    [ObservableProperty]
    private string _defaultCode = string.Empty;

    [ObservableProperty]
    private string _disabledCode = string.Empty;

    [ObservableProperty]
    private string _indeterminateCode = string.Empty;

    [ObservableProperty]
    private string _multipleCode = string.Empty;

    partial void OnIsCheckedChanged(bool? value)
    {
        if (!value.HasValue) return;

        foreach (var item in Items)
        {
            item.IsChecked = value.Value;
        }
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