namespace ShadUI.Dialogs;

internal class SimpleDialogContext(DialogService service, SimpleDialogOptions options)
{
    public SimpleDialogOptions Options { get; set; } = options;

    public void Submit(SimpleDialogAction action)
    {
        if (Options.Callback is not null)
            Options.Callback?.Invoke(action);
        else
            Options.AsyncCallback?.Invoke(action);

        service.Close();
    }
}