using Avalonia;
using Avalonia.Controls;

namespace ShadUI.Extensions;

/// <summary>
///     Useful extensions for the <see cref="Avalonia.Controls.ComboBox" /> class.
/// </summary>
public static class ComboBox
{
    static ComboBox()
    {
        LabelProperty.Changed.AddClassHandler<Avalonia.Controls.ComboBox>((element, args) =>
        {
            element.TemplateApplied += (sender, eventArgs) =>
            {
                var label = eventArgs.NameScope.Find<TextBlock>("PART_Label");
                if (label is null || string.IsNullOrEmpty(args.NewValue?.ToString())) return;

                label.Text = args.NewValue!.ToString();
            };
        });
    }

    /// <summary>
    ///     Override the default text box floating watermark.
    /// </summary>
    public static readonly AttachedProperty<string> LabelProperty =
        AvaloniaProperty.RegisterAttached<Avalonia.Controls.ComboBox, string>("Label", typeof(Avalonia.Controls.ComboBox));

    /// <summary>
    ///     Get the value of the <see cref="LabelProperty" />.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static string GetLabel(Avalonia.Controls.ComboBox element) => element.GetValue(LabelProperty);

    /// <summary>
    ///     Set the value of the <see cref="LabelProperty" />.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="value"></param>
    public static void SetLabel(Avalonia.Controls.ComboBox element, string value)
    {
        element.SetValue(LabelProperty, value);
    }

    /// <summary>
    ///     Show a hint text.
    /// </summary>
    public static readonly AttachedProperty<string> HintProperty =
        AvaloniaProperty.RegisterAttached<Avalonia.Controls.ComboBox, string>("Hint", typeof(Avalonia.Controls.ComboBox));

    /// <summary>
    ///     Get the value of the <see cref="HintProperty" />.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static string GetHint(Avalonia.Controls.ComboBox element) => element.GetValue(HintProperty);

    /// <summary>
    ///     Set the value of the <see cref="HintProperty" />.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="value"></param>
    public static void SetHint(Avalonia.Controls.ComboBox element, string value)
    {
        element.SetValue(HintProperty, value);
    }
}