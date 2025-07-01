using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShadUI.Demo.ViewModels.Examples.DataTable;

namespace ShadUI.Demo.ViewModels;

public sealed partial class DataTableViewModel(
    IMessenger messenger,
    BasicDataTableViewModel basic,
    GroupedDataTableViewModel grouped)
    : ViewModelBase, INavigable
{
    [RelayCommand]
    private void BackPage()
    {
        messenger.Send(new PageChangedMessage { PageType = typeof(ComboBoxViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        messenger.Send(new PageChangedMessage { PageType = typeof(DateViewModel) });
    }

    public BasicDataTableViewModel BasicDataTable { get; } = basic;
    public GroupedDataTableViewModel GroupedDataTable { get; } = grouped;

    public string Route => "data-table";
}