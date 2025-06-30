using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class TabControlViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public TabControlViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "TabControlPage.axaml");
        BasicTabCode = path.ExtractByLineRange(61, 79).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(SwitchViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(TimeViewModel) });
    }

    [ObservableProperty]
    private string _basicTabCode = string.Empty;

    public string Route => "tab-control";
}