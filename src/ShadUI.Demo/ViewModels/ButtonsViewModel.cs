using System;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ButtonsViewModel : ViewModelBase
{
    public ButtonsViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "ButtonsPage.axaml");
        PrimaryCode = path.ExtractByLineRange(30, 55).CleanIndentation();
        SecondaryCode = path.ExtractByLineRange(61, 86).CleanIndentation();
        DestructiveCode = path.ExtractByLineRange(92, 117).CleanIndentation();
        OutlineCode = path.ExtractByLineRange(123, 148).CleanIndentation();
        GhostCode = path.ExtractByLineRange(154, 179).CleanIndentation();
        IconCode = path.ExtractByLineRange(185, 212).CleanIndentation();
        DestructiveIconCode = path.ExtractByLineRange(218, 241).CleanIndentation();
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
}