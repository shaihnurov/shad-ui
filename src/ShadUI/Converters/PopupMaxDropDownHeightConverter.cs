using System;
using System.Globalization;
using Avalonia.Data.Converters;

// ReSharper disable once CheckNamespace
namespace ShadUI;

internal class PopupMaxDropDownHeightConverter : IValueConverter
{
    public static PopupMaxDropDownHeightConverter Instance => new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is double maxHeight ? maxHeight + 9 : double.NaN;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}