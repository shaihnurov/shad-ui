using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ShadUI.Demo.Converters;

public class TimeOnlyToTimeSpanConverter : IValueConverter
{
    public static readonly TimeOnlyToTimeSpanConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is TimeOnly timeOnly ? timeOnly.ToTimeSpan() : null;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is TimeSpan timeSpan ? TimeOnly.FromTimeSpan(timeSpan) : null;
}