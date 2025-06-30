using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ComboBoxViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public ComboBoxViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "ComboBoxPage.axaml");
        SelectComboBoxCode = path.ExtractByLineRange(62, 78).CleanIndentation();
        SelectComboBoxDisabledCode = path.ExtractByLineRange(84, 87).CleanIndentation();
        FormValidationCode = path.ExtractByLineRange(93, 115).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(ColorViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(DataTableViewModel) });
    }

    [ObservableProperty]
    private string _selectComboBoxCode = string.Empty;

    [ObservableProperty]
    private string _selectComboBoxDisabledCode = string.Empty;

    [ObservableProperty]
    private string _formValidationCode = string.Empty;

    [ObservableProperty]
    private string[] _items =
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

    public string Route => "combobox";
}