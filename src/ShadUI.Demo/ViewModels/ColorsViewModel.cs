using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ColorsViewModel : ViewModelBase
{
    public ColorsViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "ColorsPage.axaml");
        ColorViewCode = path.ExtractByLineRange(35, 40).CleanIndentation();
        ColorPickerCode = path.ExtractByLineRange(46, 51).CleanIndentation();
        ColorPickerDisabledCode = path.ExtractByLineRange(57, 63).CleanIndentation();
    }

    [ObservableProperty]
    private string _selectedColor = "#3b82f6";

    [ObservableProperty]
    private string _colorViewCode = string.Empty;

    [ObservableProperty]
    private string _colorPickerCode = string.Empty;

    [ObservableProperty]
    private string _colorPickerDisabledCode = string.Empty;
}