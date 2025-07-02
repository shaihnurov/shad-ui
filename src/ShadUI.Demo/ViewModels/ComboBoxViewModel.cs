using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShadUI.Demo.ViewModels.Examples.ComboBox;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ComboBoxViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;

    public ComboBoxViewModel(IMessenger messenger, FormComboBoxViewModel form)
    {
        _messenger = messenger;
        Form = form;

        var xamlPath = Path.Combine(AppContext.BaseDirectory, "views", "ComboBoxPage.axaml");
        SelectComboBoxCode = xamlPath.ExtractByLineRange(58, 74).CleanIndentation();
        SelectComboBoxDisabledCode = xamlPath.ExtractByLineRange(77, 80).CleanIndentation();
    }

    [RelayCommand]
    private void BackPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(ColorViewModel) });
    }

    [RelayCommand]
    private void NextPage()
    {
        _messenger.Send(new PageChangedMessage { PageType = typeof(DataTableViewModel) });
    }

    [ObservableProperty]
    private string _selectComboBoxCode = string.Empty;

    [ObservableProperty]
    private string _selectComboBoxDisabledCode = string.Empty;

    [ObservableProperty]
    private FormComboBoxViewModel _form;

    public string Route => "combobox";
}