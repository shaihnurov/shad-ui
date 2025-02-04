using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace ShadUI.Extensions;

/// <summary>
///     Provides attached properties and methods for assisting with control behaviors.
/// </summary>
public static class ControlAssist
{
    static ControlAssist()
    {
        LabelProperty.Changed.AddClassHandler<TemplatedControl>((control, args) =>
        {
            control.TemplateApplied += (sender, eventArgs) =>
            {

                var label = eventArgs.NameScope.Find<TextBlock>("PART_Label");
                if (label is null || string.IsNullOrEmpty(args.NewValue?.ToString())) return;

                if(sender is Avalonia.Controls.TextBox tb) tb.UseFloatingWatermark = true;

                label.Text = args.NewValue!.ToString();
            };
        });
    }

    /// <summary>
    ///     Add a label or override the default control floating watermark.
    /// </summary>
    public static readonly AttachedProperty<string> LabelProperty =
        AvaloniaProperty.RegisterAttached<TemplatedControl, string>("Label", typeof(TemplatedControl));

    /// <summary>
    ///     Get the value of the <see cref="LabelProperty" />.
    /// </summary>
    /// <param name="control"></param>
    /// <returns></returns>
    public static string GetLabel(TemplatedControl control) => control.GetValue(LabelProperty);

    /// <summary>
    ///     Set the value of the <see cref="LabelProperty" />.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="value"></param>
    public static void SetLabel(TemplatedControl control, string value)
    {
        control.SetValue(LabelProperty, value);
    }

    /// <summary>
    ///     Show a hint text.
    /// </summary>
    public static readonly AttachedProperty<string> HintProperty =
        AvaloniaProperty.RegisterAttached<TemplatedControl, string>("Hint", typeof(TemplatedControl));

    /// <summary>
    ///     Get the value of the <see cref="HintProperty" />.
    /// </summary>
    /// <param name="control"></param>
    /// <returns></returns>
    public static string GetHint(TemplatedControl control) => control.GetValue(HintProperty);

    /// <summary>
    ///     Set the value of the <see cref="HintProperty" />.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="value"></param>
    public static void SetHint(TemplatedControl control, string value)
    {
        control.SetValue(HintProperty, value);
    }

    /// <summary>
    ///     Indicates whether the control should show progress.
    /// </summary>
    public static readonly AttachedProperty<bool> ShowProgressProperty =
        AvaloniaProperty.RegisterAttached<TemplatedControl, bool>("ShowProgress", typeof(TemplatedControl));

    /// <summary>
    ///     Gets the value of the <see cref="ShowProgressProperty" />.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <returns>The value of the <see cref="ShowProgressProperty" />.</returns>
    public static bool GetShowProgress(TemplatedControl control) => control.GetValue(ShowProgressProperty);

    /// <summary>
    ///     Sets the value of the <see cref="ShowProgressProperty" />.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="value">The value to set.</param>
    public static void SetShowProgress(TemplatedControl control, bool value)
    {
        control.SetValue(ShowProgressProperty, value);
    }
}