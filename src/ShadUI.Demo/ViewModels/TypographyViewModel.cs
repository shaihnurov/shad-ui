using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShadUI.Demo.ViewModels.Examples.Typography;

namespace ShadUI.Demo.ViewModels;

public sealed partial class TypographyViewModel : ViewModelBase, INavigable
{
    private readonly IMessenger _messenger;
    private readonly TextBlockViewModel _textBlock;
    private readonly SelectableTextBlockViewModel _selectableTextBlock;
    private readonly LabelViewModel _label;

    public TypographyViewModel(
        IMessenger messenger,
        TextBlockViewModel textBlock,
        SelectableTextBlockViewModel selectableTextBlock,
        LabelViewModel label)
    {
        _messenger = messenger;
        _textBlock = textBlock;
        _selectableTextBlock = selectableTextBlock;
        _label = label;
        PropertyChanged += OnPropertyChanged;

        Content = _textBlock;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SelectedControl))
        {
            switch (SelectedControl)
            {
                case "Text Block":
                    Content = _textBlock;
                    break;
                case "Selectable Text Block":
                    Content = _selectableTextBlock;
                    break;
                case "Label":
                    Content = _label;
                    break;
            }
        }
    }

    [ObservableProperty]
    private string _selectedControl = "Text Block";

    [ObservableProperty]
    private string[] _controls =
    [
        "Text Block",
        "Selectable Text Block",
        "Label"
    ];

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
    private object _content = null!;

    public string Route => "typography";
}