using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ShadUI.Converters;

public sealed class BackgroundToForegroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Color color) return Color.Parse("#FAFAFA");

        var luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;
        return luminance > 0.5 ? Color.Parse("#18181B") : Color.Parse("#FAFAFA");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}