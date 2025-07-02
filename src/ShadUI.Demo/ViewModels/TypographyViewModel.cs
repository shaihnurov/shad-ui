using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class TypographyViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public TypographyViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "TypographyPage.axaml");
        H1Code = path.ExtractByLineRange(57, 60).CleanIndentation();
        H2Code = path.ExtractByLineRange(63, 66).CleanIndentation();
        H3Code = path.ExtractByLineRange(69, 72).CleanIndentation();
        H4Code = path.ExtractByLineRange(75, 78).CleanIndentation();
        PCode = path.ExtractByLineRange(81, 84).CleanIndentation();
        LeadCode = path.ExtractByLineRange(87, 90).CleanIndentation();
        LargeCode = path.ExtractByLineRange(93, 96).CleanIndentation();
        SmallCode = path.ExtractByLineRange(99, 102).CleanIndentation();
        CaptionCode = path.ExtractByLineRange(105, 108).CleanIndentation();
        MutedCode = path.ExtractByLineRange(111, 114).CleanIndentation();
        ErrorCode = path.ExtractByLineRange(117, 120).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(ThemeViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(AvatarViewModel) });
    }

    [ObservableProperty]
    private string _h1Code = string.Empty;

    [ObservableProperty]
    private string _h2Code = string.Empty;

    [ObservableProperty]
    private string _h3Code = string.Empty;

    [ObservableProperty]
    private string _h4Code = string.Empty;

    [ObservableProperty]
    private string _pCode = string.Empty;

    [ObservableProperty]
    private string _leadCode = string.Empty;

    [ObservableProperty]
    private string _largeCode = string.Empty;

    [ObservableProperty]
    private string _smallCode = string.Empty;

    [ObservableProperty]
    private string _captionCode = string.Empty;

    [ObservableProperty]
    private string _mutedCode = string.Empty;

    [ObservableProperty]
    private string _errorCode = string.Empty;

    public string Route => "typography";
}