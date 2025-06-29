using System;
using System.Globalization;
using Avalonia.Data.Converters;

// ReSharper disable once CheckNamespace
namespace ShadUI;

/// <summary>
///     Returns a width of 24 if the value is true, otherwise 0.
/// </summary>
internal class BoolToWidthProgressConverter : IValueConverter
{
    public static readonly BoolToWidthProgressConverter Instance = new();

    /// <summary>
    ///     Convert a boolean value to a width of 24 or 0.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return 0;

        return (bool) value ? 24 : 0;
    }

    /// <summary>
    ///     Convert a width of 24 or 0 to a boolean value.
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

/// <summary>
///     Returns a width of 0 if the value is true, otherwise 24.
/// </summary>
internal class BoolToInverseWidthProgressConverter : IValueConverter
{
    public static readonly BoolToInverseWidthProgressConverter Instance = new();

    /// <summary>
    ///     Convert a boolean value to a width of 0 or 36.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return 24;

        return (bool) value ? 0 : 24;
    }

    /// <summary>
    ///     Convert a width of 0 or 24 to a boolean value.
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