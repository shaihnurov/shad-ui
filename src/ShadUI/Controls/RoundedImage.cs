using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ShadUI.Controls;

/// <summary>
/// Custom Image control with rounded corners.
/// </summary>
public class RoundedImage : Image
{
    /// <summary>
    /// Attached property for setting the corner radius of the image.
    /// </summary>
    public static readonly AttachedProperty<double> CornerRadiusProperty =
        AvaloniaProperty.RegisterAttached<RoundedImage, double>(
            "CornerRadius", typeof(RoundedImage), 5);

    /// <summary>
    /// Sets the corner radius of the image.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="parameter"></param>
    public static void SetCornerRadius(AvaloniaObject element, double parameter)
    {
        element.SetValue(CornerRadiusProperty, parameter);
    }

    /// <summary>
    /// Gets the corner radius of the image.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static double GetCornerRadius(AvaloniaObject element) => element.GetValue(CornerRadiusProperty);

    /// <summary>
    /// Measures the control.
    /// </summary>
    /// <param name="availableSize">The available size.</param>
    /// <returns>The desired size of the control.</returns>
    protected override Size MeasureOverride(Size availableSize)
    {
        var source = Source;
        Size result = new();

        if (source != null) result = Stretch.CalculateSize(availableSize, source.Size, StretchDirection);
        Clip = new RectangleGeometry(new Rect(0, 0, result.Width, result.Height), GetCornerRadius(this), GetCornerRadius(this));
        return result;
    }
}