using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ShadUI.Converters;

/// <summary>
///     Returns a width of 40 if the value is true, otherwise 0.
/// </summary>
public class BoolToWidthProgressConverter : IValueConverter
{
    /// <summary>
    ///     Convert a boolean value to a width of 40 or 0.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return 0;

        return (bool) value ? 40 : 0;
    }

    /// <summary>
    ///     Convert a width of 40 or 0 to a boolean value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}