using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ShadUI.Converters;

public class BoolToWidthProgressConverter : IValueConverter
{
    public static readonly BoolToWidthProgressConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return 0;

        return (bool) value ? 40 : 0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}