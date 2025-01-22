using Avalonia;
using Avalonia.Controls;

namespace ShadUI.Demo.Controls;

public class ControlBlock : ContentControl
{
    public static readonly StyledProperty<string?> CodeProperty =
        AvaloniaProperty.Register<CodeTextBlock, string?>(nameof(Code));

    public string? Code
    {
        get => GetValue(CodeProperty);
        set => SetValue(CodeProperty, value);
    }

    public static readonly StyledProperty<string?> LanguageProperty =
        AvaloniaProperty.Register<CodeTextBlock, string?>(nameof(Language), defaultValue: "xaml");

    public string? Language
    {
        get => GetValue(LanguageProperty);
        set => SetValue(LanguageProperty, value);
    }

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<CodeTextBlock, string?>(nameof(Title));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
}