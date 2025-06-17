using System;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShadUI.Demo.ViewModels;

public sealed partial class MiscellaneousViewModel : ViewModelBase
{
    public MiscellaneousViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "views", "MiscellaneousPage.axaml");
        BusyAreaCode = path.ExtractByLineRange(35, 50).CleanIndentation();
        SkeletonCode = path.ExtractByLineRange(56, 75).CleanIndentation();
    }

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _busyAreaCode = string.Empty;

    [RelayCommand]
    private async Task ToggleBusy()
    {
        IsBusy = true;
        await Task.Delay(3000);
        IsBusy = false;
    }

    [ObservableProperty]
    private string _skeletonCode = string.Empty;
}