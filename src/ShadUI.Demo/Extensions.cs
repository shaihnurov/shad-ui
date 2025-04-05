using ShadUI.Demo.ViewModels;
using ShadUI.Demo.Views;
using ShadUI.Dialogs;

namespace ShadUI.Demo;

public static class Extensions
{
    public static ServiceProvider RegisterDialogs(this ServiceProvider service)
    {
        var dialogService = service.GetService<DialogManager>();
        dialogService.Register<LoginContent, LoginViewModel>();
        dialogService.Register<AboutContent, AboutViewModel>();

        return service;
    }
}