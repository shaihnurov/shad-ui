using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class SliderViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public SliderViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "SliderPage.axaml");
        DefaultSliderCode = path.ExtractByLineRange(58, 62).CleanIndentation();
        DisabledSliderCode = path.ExtractByLineRange(65, 70).CleanIndentation();
        TickEnabledSliderCode = path.ExtractByLineRange(73, 79).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(SidebarViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(SwitchViewModel) });
    }

    [ObservableProperty]
    private string _defaultSliderCode = string.Empty;

    [ObservableProperty]
    private string _disabledSliderCode = string.Empty;

    [ObservableProperty]
    private string _tickEnabledSliderCode = string.Empty;

    public string Route => "slider";
}