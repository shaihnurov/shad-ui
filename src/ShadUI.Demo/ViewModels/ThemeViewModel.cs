using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ThemeViewModel(IMessenger messenger) : ViewModelBase, INavigable
{
    [RelayCommand]
    private void BackPage()
    {
        messenger.Send(new PageChangedMessage { PageType = typeof(DashboardViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        messenger.Send(new PageChangedMessage { PageType = typeof(TypographyViewModel) });
    }

    public string Route => "theme";
}