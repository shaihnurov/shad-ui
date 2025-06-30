using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class TabControlViewModel : ViewModelBase
{
    public TabControlViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "TabControlPage.axaml");
        BasicTabCode = path.ExtractByLineRange(33, 51).CleanIndentation();
    }

    [ObservableProperty]
    private string _basicTabCode = string.Empty;
}