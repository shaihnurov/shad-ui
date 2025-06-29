using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

// ReSharper disable once CheckNamespace
namespace ShadUI;

/// <summary>
///     Converts the value of the window background to a margin.
/// </summary>
internal class WindowBackgroundToMarginConverter : IValueConverter
{
    /// <summary>
    ///     Returns the instance of the <see cref="WindowBackgroundToMarginConverter" />.
    /// </summary>
    public static readonly WindowBackgroundToMarginConverter Instance = new();

    /// <summary>
    ///     Converts the value of the window background to a margin.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null) return new Thickness(0);
        if ((bool)value == false) return new Thickness(10, 5, 0, 10);

        return new Thickness(0);
    }

    /// <summary>
    ///     Converts the value of margin to the window background.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}