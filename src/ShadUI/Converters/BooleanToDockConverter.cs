using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace ShadUI.Converters;

internal sealed class BooleanToDockConverter : IValueConverter
{
    public static readonly BooleanToDockConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool b) return Dock.Left;

        return b ? Dock.Right : Dock.Left;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}