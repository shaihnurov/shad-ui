using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class SwitchViewModel : ViewModelBase
{
    public SwitchViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "SwitchPage.axaml");
        EnableCode = path.ExtractByLineRange(34, 52).CleanIndentation();
        DisableCode = path.ExtractByLineRange(58, 76).CleanIndentation();
        RightAlignedCode = path.ExtractByLineRange(82, 100).CleanIndentation();
    }

    [ObservableProperty]
    private string _enableCode = string.Empty;

    [ObservableProperty]
    private string _disableCode = string.Empty;

    [ObservableProperty]
    private string _rightAlignedCode = string.Empty;
}