using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class AvatarViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public AvatarViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "AvatarPage.axaml");
        UsageCode = path.ExtractByLineRange(59, 63).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(TypographyViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(ButtonViewModel) });
    }

    [ObservableProperty]
    private string _usageCode = string.Empty;

    public string Route => "avatar";
}