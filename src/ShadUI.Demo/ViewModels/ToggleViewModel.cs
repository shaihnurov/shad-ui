using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ToggleViewModel : ViewModelBase
{
    public ToggleViewModel()
    {
        var path = System.IO.Path.Combine(AppContext.BaseDirectory, "views", "TogglePage.axaml");
        DefaultCode = path.ExtractByLineRange(30,35).CleanIndentation();
        OutlineCode = path.ExtractByLineRange(41, 46).CleanIndentation();
        WithTextCode = path.ExtractByLineRange(52, 60).CleanIndentation();
        DisableCode = path.ExtractByLineRange(66, 72).CleanIndentation();
    }

    [ObservableProperty]
    private string _defaultCode = string.Empty;

    [ObservableProperty]
    private string _outlineCode = string.Empty;

    [ObservableProperty]
    private string _withTextCode = string.Empty;

    [ObservableProperty]
    private string _disableCode = string.Empty;
}