using System;
using System.Globalization;
using Avalonia.Data.Converters;

// ReSharper disable once CheckNamespace
namespace ShadUI;

internal sealed class ClockIdentifierToIsVisibleConverter : IValueConverter
{
    public static ClockIdentifierToIsVisibleConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string identifier)
            throw new ArgumentException("Value must be a string");

        if (identifier != "12HourClock" && identifier != "24HourClock")
            throw new ArgumentException("Value must be either 12HourClock or 24HourClock");

        return identifier == "12HourClock";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}