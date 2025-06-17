using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class TabsViewModel : ViewModelBase
{
    public TabsViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "TabsPage.axaml");
        BasicTabCode = path.ExtractByLineRange(34, 54).CleanIndentation();
    }

    [ObservableProperty]
    private string _basicTabCode = string.Empty;
}