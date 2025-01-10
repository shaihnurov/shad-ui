using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using ShadUI.Controls;
using ShadUI.Enums;

namespace ShadUI.Converters;

/// <summary>
/// Convert a boolean value to a <see cref="Panel"/> or <see cref="Loading"/>.
/// </summary>
public class ProgressToContentConverter : IValueConverter
{
    /// <summary>
    /// Returns a new instance of the <see cref="ProgressToContentConverter"/> class.
    /// </summary>
    public static readonly ProgressToContentConverter Instance = new();

    /// <summary>
    /// Convert a boolean value to a <see cref="Panel"/> or <see cref="Loading"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object? Convert(object? value, Type targetType, object? parameter,
        CultureInfo culture)
    {
        if (value is true)
            return new Loading { LoadingStyle = LoadingStyle.Simple };

        return new Panel();
    }

    /// <summary>
    /// Convert a <see cref="Panel"/> or <see cref="Loading"/> to a boolean value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public object ConvertBack(object? value, Type targetType,
        object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}