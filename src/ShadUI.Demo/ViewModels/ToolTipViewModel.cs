using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ToolTipViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public ToolTipViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "ToolTipPage.axaml");
        UsageCode = path.ExtractByLineRange(58, 65).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(ToggleViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(MiscellaneousViewModel) });
    }
    
    [ObservableProperty]
    private string _usageCode = string.Empty;

    public string Route => "tooltip";
}