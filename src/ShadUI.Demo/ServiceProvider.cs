using Avalonia;
using Jab;
using ShadUI.Demo.ViewModels;
using ShadUI.Themes;

namespace ShadUI.Demo;

[ServiceProvider]
[Transient<AvatarsViewModel>]
[Transient<ButtonsViewModel>]
[Transient<CardsViewModel>]
[Transient<CheckBoxesViewModel>]
[Transient<ComboBoxesViewModel>]
[Transient<DashboardViewModel>]
[Transient<DialogsViewModel>]
[Transient<InputViewModel>]
[Transient<LoginViewModel>]
[Transient<MenuViewModel>]
[Transient<MiscellaneousViewModel>]
[Transient<SlidersViewModel>]
[Transient<TabsViewModel>]
[Transient<DateTimeViewModel>]
[Transient<ToastsViewModel>]
[Transient<SwitchViewModel>]
[Transient<TogglesViewModel>]
[Transient<ToolTipViewModel>]
[Transient<TypographyViewModel>]
[Transient<MainWindowViewModel>]
[Import<IUtilitiesModule>]
[Singleton(typeof(ThemeWatcher), Factory = nameof(ThemeWatcherFactory))]
public partial class ServiceProvider
{
    public ThemeWatcher ThemeWatcherFactory() => new(Application.Current!);
}