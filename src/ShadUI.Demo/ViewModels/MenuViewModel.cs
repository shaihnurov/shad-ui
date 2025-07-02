using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class MenuViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public MenuViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "MenuPage.axaml");
        SimpleDropdownCode = path.ExtractByLineRange(58, 189).CleanIndentation();
        MenuBarCode = path.ExtractByLineRange(192, 267).CleanIndentation();
        DropDownCode = path.ExtractByLineRange(271, 434).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(InputViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(NumericViewModel) });
    }

    [ObservableProperty]
    private string _simpleDropdownCode = string.Empty;

    [ObservableProperty]
    private string _menuBarCode = string.Empty;

    [ObservableProperty]
    private string _dropDownCode = string.Empty;

    public string Route => "menu";
}