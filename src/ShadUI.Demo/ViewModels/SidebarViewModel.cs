using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class SidebarViewModel : ViewModelBase
{
    public SidebarViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "SidebarPage.axaml");
        DefaultCode = path.ExtractByLineRange(38, 452).CleanIndentation();
    }

    [ObservableProperty]
    private string _defaultCode = string.Empty;
}