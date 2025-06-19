using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ToolTipViewModel : ViewModelBase
{
    public ToolTipViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "ToolTipPage.axaml");
        UsageCode = path.ExtractByLineRange(33, 40).CleanIndentation();
    }

    [ObservableProperty]
    private string _usageCode = string.Empty;
}