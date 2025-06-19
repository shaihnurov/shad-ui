using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class AvatarViewModel : ViewModelBase
{
    public AvatarViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "AvatarPage.axaml");
        UsageCode = path.ExtractByLineRange(34, 38).CleanIndentation();
    }

    [ObservableProperty]
    private string _usageCode = string.Empty;
}