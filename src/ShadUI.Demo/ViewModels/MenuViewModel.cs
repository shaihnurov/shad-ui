using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class MenuViewModel : ViewModelBase
{
    public MenuViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "MenuPage.axaml");
        SimpleDropdownCode = path.ExtractByLineRange(36, 167).CleanIndentation();
        MenuBarCode = path.ExtractByLineRange(173, 249).CleanIndentation();
        DropDownCode = path.ExtractByLineRange(256, 426).CleanIndentation();
    }

    [ObservableProperty]
    private string _simpleDropdownCode = string.Empty;

    [ObservableProperty]
    private string _menuBarCode = string.Empty;

    [ObservableProperty]
    private string _dropDownCode = string.Empty;
}