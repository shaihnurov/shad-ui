using Avalonia;
using Avalonia.Controls;

namespace ShadUI.Extensions;

/// <summary>
///     Useful extensions for the <see cref="Avalonia.Controls.TextBox" /> class.
/// </summary>
public static class TextBoxExt
{
    static TextBoxExt()
    {
        LabelProperty.Changed.AddClassHandler<TextBox>((textBox, args) =>
        {
            textBox.TemplateApplied += (sender, eventArgs) =>
            {
                var tb = (TextBox) sender;

                var label = eventArgs.NameScope.Find<TextBlock>("PART_Label");
                if (label is null || string.IsNullOrEmpty(args.NewValue?.ToString())) return;

                tb.UseFloatingWatermark = true;
                label.Text = args.NewValue!.ToString();
            };
        });
    }

    /// <summary>
    ///     Override the default text box floating watermark.
    /// </summary>
    public static readonly AttachedProperty<string> LabelProperty =
        AvaloniaProperty.RegisterAttached<TextBox, string>("Label", typeof(TextBox));

    /// <summary>
    ///     Get the value of the <see cref="LabelProperty" />.
    /// </summary>
    /// <param name="textBox"></param>
    /// <returns></returns>
    public static string GetLabel(TextBox textBox) => textBox.GetValue(LabelProperty);

    /// <summary>
    ///     Set the value of the <see cref="LabelProperty" />.
    /// </summary>
    /// <param name="textBox"></param>
    /// <param name="value"></param>
    public static void SetLabel(TextBox textBox, string value)
    {
        textBox.SetValue(LabelProperty, value);
    }

    /// <summary>
    ///     Show a hint text.
    /// </summary>
    public static readonly AttachedProperty<string> HintProperty =
        AvaloniaProperty.RegisterAttached<TextBox, string>("Hint", typeof(TextBox));

    /// <summary>
    ///     Get the value of the <see cref="HintProperty" />.
    /// </summary>
    /// <param name="textBox"></param>
    /// <returns></returns>
    public static string GetHint(TextBox textBox) => textBox.GetValue(HintProperty);

    /// <summary>
    ///     Set the value of the <see cref="HintProperty" />.
    /// </summary>
    /// <param name="textBox"></param>
    /// <param name="value"></param>
    public static void SetHint(TextBox textBox, string value)
    {
        textBox.SetValue(HintProperty, value);
    }
}