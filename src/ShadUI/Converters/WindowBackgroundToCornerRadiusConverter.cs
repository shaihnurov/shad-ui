using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

// ReSharper disable once CheckNamespace
namespace ShadUI;

/// <summary>
///     Converts the value of the window background to a corner radius.
/// </summary>
internal class WindowBackgroundToCornerRadiusConverter : IValueConverter
{
    /// <summary>
    ///     Returns the instance of the <see cref="WindowBackgroundToCornerRadiusConverter" />.
    /// </summary>
    public static readonly WindowBackgroundToCornerRadiusConverter Instance = new();

    /// <summary>
    ///     Converts the value of the window background to a corner radius.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null) return new CornerRadius(0);

        if ((bool)value == false) return new CornerRadius(17);

        return new CornerRadius(0);
    }

    /// <summary>
    ///     Converts the value of corner radius to the window background.
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