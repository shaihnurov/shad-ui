using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ComboBoxesViewModel : ViewModelBase
{
    public ComboBoxesViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "ComboBoxesPage.axaml");
        SelectComboBoxCode = path.ExtractByLineRange(36, 44).CleanIndentation();
        SelectComboBoxDisabledCode = path.ExtractByLineRange(50, 55).CleanIndentation();
        FormValidationCode = path.ExtractByLineRange(61, 83).CleanIndentation();
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
}