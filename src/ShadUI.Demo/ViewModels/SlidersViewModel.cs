using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class SlidersViewModel : ViewModelBase
{
    public SlidersViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "SlidersPage.axaml");
        DefaultSliderCode = path.ExtractByLineRange(34, 38).CleanIndentation();
        DisabledSliderCode = path.ExtractByLineRange(44, 49).CleanIndentation();
        TickEnabledSliderCode = path.ExtractByLineRange(55, 61).CleanIndentation();
    }

    [ObservableProperty]
    private string _defaultSliderCode = string.Empty;

    [ObservableProperty]
    private string _disabledSliderCode = string.Empty;

    [ObservableProperty]
    private string _tickEnabledSliderCode = string.Empty;
}