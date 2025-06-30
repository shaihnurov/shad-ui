using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class SidebarViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public SidebarViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "SidebarPage.axaml");
        DefaultCode = path.ExtractByLineRange(63, 477).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(NumericViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(SliderViewModel) });
    }

    [ObservableProperty]
    private string _defaultCode = string.Empty;

    public string Route => "sidebar";
}