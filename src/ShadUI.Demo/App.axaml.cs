using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using ShadUI.Demo.ViewModels;
using ShadUI.Demo.Views;
using ShadUI.Themes;

namespace ShadUI.Demo;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

        DisableAvaloniaDataAnnotationValidation();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddServices();

        var serviceProvider = serviceCollection
            .BuildServiceProvider()
            .RegisterDialogs();

        var themeWatcher = serviceProvider.GetRequiredService<ThemeWatcher>();
        themeWatcher.Initialize();
        var viewModel = serviceProvider.GetRequiredService<MainWindowViewModel>();
        viewModel.Initialize();

        desktop.MainWindow = new MainWindow { DataContext = viewModel };

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove) BindingPlugins.DataValidators.Remove(plugin);
    }
}