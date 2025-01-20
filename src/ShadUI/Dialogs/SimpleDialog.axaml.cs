using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ShadUI.Dialogs;

internal partial class SimpleDialog : UserControl
{
    public SimpleDialog(SimpleDialogContext context)
    {
        DataContext = context;
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var el = ButtonContainer.Children
            .FirstOrDefault(x => x is Button { Content: not null } button && button.Content.ToString() != "");

        el?.Focus();
    }

    private void OnSubmit(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button) return;
        if (DataContext is not SimpleDialogContext context) return;
        if (button.Tag is not SimpleDialogAction action) return;

        context.Submit(action);
    }
}