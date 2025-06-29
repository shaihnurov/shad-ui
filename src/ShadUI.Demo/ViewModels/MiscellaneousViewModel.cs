using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShadUI.Demo.ViewModels;

public sealed partial class MiscellaneousViewModel : ViewModelBase
{
    public MiscellaneousViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "MiscellaneousPage.axaml");
        BusyAreaCode = path.ExtractByLineRange(35, 50).CleanIndentation();
        ListBoxCode = path.ExtractByLineRange(56, 82).CleanIndentation();
        SkeletonCode = path.ExtractByLineRange(88, 107).CleanIndentation();

        WebFrameworksSelectionModel.SelectionChanged += SelectedWebFrameworksChanged;
        _selectedWebFrameworks.CollectionChanged += OnSelectedWebFrameworksOnCollectionChanged;
    }

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _busyAreaCode = string.Empty;

    [RelayCommand]
    private async Task ToggleBusy()
    {
        IsBusy = true;
        await Task.Delay(3000);
        IsBusy = false;
    }

    private SelectionMode _selectionMode;

    public SelectionMode SelectionMode
    {
        get => _selectionMode;
        set => SetProperty(ref _selectionMode, value);
    }

    [ObservableProperty]
    private SelectionMode[] _selectionModes =
    [
        SelectionMode.Single,
        SelectionMode.Multiple,
        SelectionMode.Toggle,
        SelectionMode.AlwaysSelected
    ];

    [ObservableProperty]
    private string _selectedWebFrameworkCount = "No frameworks selected";

    [ObservableProperty]
    private string[] _webFrameworks =
    [
        "Angular",
        "Astro",
        "Lit",
        "Next.js",
        "Nuxt.js",
        "Preact",
        "Qwik",
        "React",
        "Remix",
        "SolidJS",
        "Svelte",
        "SvelteKit",
        "Vue.js"
    ];

    [ObservableProperty]
    private ISelectionModel _webFrameworksSelectionModel = new SelectionModel<string>();

    [ObservableProperty]
    private string _listBoxCode = string.Empty;

    private readonly ObservableCollection<string> _selectedWebFrameworks = [];

    private void OnSelectedWebFrameworksOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        SelectedWebFrameworkCount = _selectedWebFrameworks.Count switch
        {
            0 => "No frameworks selected",
            1 => $"1 framework selected: {_selectedWebFrameworks[0]}",
            _ => $"{_selectedWebFrameworks.Count} frameworks selected"
        };
    }

    private void SelectedWebFrameworksChanged(object? sender, SelectionModelSelectionChangedEventArgs e)
    {
        var selectedItems = e.SelectedItems;
        var deselectedItems = e.DeselectedItems;

        if (selectedItems.Count > 0)
        {
            foreach (var i in selectedItems)
            {
                if (i is not string s) continue;
                if (!_selectedWebFrameworks.Contains(s)) _selectedWebFrameworks.Add(s);
            }
        }

        if (deselectedItems.Count > 0)
        {
            foreach (var i in deselectedItems)
            {
                if (i is not string s) continue;
                _selectedWebFrameworks.Remove(s);
            }
        }
    }

    [ObservableProperty]
    private string _skeletonCode = string.Empty;
}