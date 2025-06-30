using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class CardViewModel : ViewModelBase
{
    public CardViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "CardPage.axaml");
        UsageCode = path.ExtractByLineRange(34, 74).CleanIndentation();
    }

    [ObservableProperty]
    private string _usageCode = string.Empty;
}