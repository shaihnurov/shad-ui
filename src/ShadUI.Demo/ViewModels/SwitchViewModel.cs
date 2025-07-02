using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class SwitchViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public SwitchViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "SwitchPage.axaml");
        EnableCode = path.ExtractByLineRange(58, 76).CleanIndentation();
        DisableCode = path.ExtractByLineRange(79, 97).CleanIndentation();
        RightAlignedCode = path.ExtractByLineRange(100, 118).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(SliderViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(TabControlViewModel) });
    }

    [ObservableProperty]
    private string _enableCode = string.Empty;

    [ObservableProperty]
    private string _disableCode = string.Empty;

    [ObservableProperty]
    private string _rightAlignedCode = string.Empty;

    public string Route => "switch";
}