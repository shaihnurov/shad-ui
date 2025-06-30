using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class CardViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public CardViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "CardPage.axaml");
        UsageCode = path.ExtractByLineRange(61, 101).CleanIndentation();
    }
    
    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(ButtonViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(CheckBoxViewModel) });
    }

    [ObservableProperty]
    private string _usageCode = string.Empty;

    public string Route => "card";
}