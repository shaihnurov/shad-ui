using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class TypographyViewModel : ViewModelBase
{
    public TypographyViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "TypographyPage.axaml");
        H1Code = path.ExtractByLineRange(29, 32).CleanIndentation();
        H2Code = path.ExtractByLineRange(38, 41).CleanIndentation();
        H3Code = path.ExtractByLineRange(47, 50).CleanIndentation();
        H4Code = path.ExtractByLineRange(56, 59).CleanIndentation();
        PCode = path.ExtractByLineRange(65, 68).CleanIndentation();
        LeadCode = path.ExtractByLineRange(74, 77).CleanIndentation();
        LargeCode = path.ExtractByLineRange(83, 86).CleanIndentation();
        SmallCode = path.ExtractByLineRange(92, 95).CleanIndentation();
        CaptionCode = path.ExtractByLineRange(101, 104).CleanIndentation();
        MutedCode = path.ExtractByLineRange(110, 113).CleanIndentation();
        ErrorCode = path.ExtractByLineRange(119, 122).CleanIndentation();
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
}