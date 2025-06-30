using System;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ButtonViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public ButtonViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "ButtonPage.axaml");
        PrimaryCode = path.ExtractByLineRange(61, 86).CleanIndentation();
        SecondaryCode = path.ExtractByLineRange(92, 117).CleanIndentation();
        DestructiveCode = path.ExtractByLineRange(123, 148).CleanIndentation();
        OutlineCode = path.ExtractByLineRange(154, 179).CleanIndentation();
        GhostCode = path.ExtractByLineRange(185, 212).CleanIndentation();
        IconCode = path.ExtractByLineRange(216, 243).CleanIndentation();
        DestructiveIconCode = path.ExtractByLineRange(249, 272).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(AvatarViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(CardViewModel) });
    }

    [ObservableProperty]
    private bool _isBusy;

    [RelayCommand]
    private async Task SubmitAsync()
    {
        IsBusy = true;
        await Task.Delay(5000);
        IsBusy = false;
    }

    [ObservableProperty]
    private string _primaryCode = string.Empty;

    [ObservableProperty]
    private string _secondaryCode = string.Empty;

    [ObservableProperty]
    private string _destructiveCode = string.Empty;

    [ObservableProperty]
    private string _outlineCode = string.Empty;

    [ObservableProperty]
    private string _ghostCode = string.Empty;

    [ObservableProperty]
    private string _iconCode = string.Empty;

    [ObservableProperty]
    private string _destructiveIconCode = string.Empty;

    public string Route => "button";
}