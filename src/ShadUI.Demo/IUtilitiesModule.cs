using Jab;
using ShadUI.Dialogs;
using ShadUI.Toasts;

namespace ShadUI.Demo;

[ServiceProviderModule]
[Singleton<DialogManager>]
[Singleton<ToastManager>]
public interface IUtilitiesModule
{
}