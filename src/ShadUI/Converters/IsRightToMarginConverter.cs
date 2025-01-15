using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace ShadUI.Converters;

internal sealed class IsRightToMarginConverter : IValueConverter
{
    public static readonly IsRightToMarginConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool b) return new Thickness(0);

        int.TryParse(parameter?.ToString(), out var gap);

        return b ? new Thickness(gap,0,0,0) : new Thickness(0,0,gap,0);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}