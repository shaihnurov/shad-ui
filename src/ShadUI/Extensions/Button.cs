using Avalonia;

namespace ShadUI.Extensions;

public static class Button
{
    public static readonly AttachedProperty<bool> ShowProgressProperty =
        AvaloniaProperty.RegisterAttached<Avalonia.Controls.Button, bool>("ShowProgress", typeof(Avalonia.Controls.Button), false);

    public static bool GetShowProgress(Avalonia.Controls.Button textBox) => textBox.GetValue(ShowProgressProperty);

    public static void SetShowProgress(Avalonia.Controls.Button textBox, bool value)
    {
        textBox.SetValue(ShowProgressProperty, value);
    }

    public static void ShowProgress(this Avalonia.Controls.Button textBox)
    {
        textBox.SetValue(ShowProgressProperty, true);
    }

    public static void HideProgress(this Avalonia.Controls.Button textBox)
    {
        textBox.SetValue(ShowProgressProperty, false);
    }
}