using System;
using System.Globalization;
using Avalonia.Data.Converters;

// ReSharper disable once CheckNamespace
namespace ShadUI;

internal class PopupMinWidthConverter : IValueConverter
{
    public static PopupMinWidthConverter Instance => new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is double minWidth ? minWidth + 4 : double.NaN;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}