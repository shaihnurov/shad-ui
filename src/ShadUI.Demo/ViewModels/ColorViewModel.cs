using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ColorViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public ColorViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "ColorPage.axaml");
        ColorViewCode = path.ExtractByLineRange(62, 67).CleanIndentation();
        ColorPickerCode = path.ExtractByLineRange(73, 78).CleanIndentation();
        ColorPickerDisabledCode = path.ExtractByLineRange(84, 90).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(CheckBoxViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(ComboBoxViewModel) });
    }

    [ObservableProperty]
    private string _selectedColor = "#3b82f6";

    [ObservableProperty]
    private string _colorViewCode = string.Empty;

    [ObservableProperty]
    private string _colorPickerCode = string.Empty;

    [ObservableProperty]
    private string _colorPickerDisabledCode = string.Empty;

    public string Route => "color";
}