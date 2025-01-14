using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ShadUI.Converters;

internal sealed class StringToIsVisibleConverter : IValueConverter
{
    /// <summary>
    ///     Returns a new instance of the <see cref="StringToIsVisibleConverter" /> class.
    /// </summary>
    public static readonly StringToIsVisibleConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        !string.IsNullOrEmpty(value?.ToString());

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}