using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using ShadUI.Controls;
using ShadUI.Enums;

namespace ShadUI.Converters;

public class ProgressToContentConverter : IValueConverter
{
    public static readonly ProgressToContentConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter,
        CultureInfo culture)
    {
        if ((bool) value)
            return new Loading { LoadingStyle = LoadingStyle.Simple };

        return new Panel();
    }

    public object ConvertBack(object? value, Type targetType,
        object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}