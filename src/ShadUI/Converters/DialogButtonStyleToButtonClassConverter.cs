using System;
using System.Globalization;
using Avalonia.Data.Converters;

// ReSharper disable once CheckNamespace
namespace ShadUI;

internal class DialogButtonStyleToButtonClassConverter : IValueConverter
{
    public static readonly DialogButtonStyleToButtonClassConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DialogButtonStyle style) return "";

        return style switch
        {
            DialogButtonStyle.Primary => "Primary",
            DialogButtonStyle.Secondary => "Secondary",
            DialogButtonStyle.Outline => "Outline",
            DialogButtonStyle.Ghost => "Ghost",
            DialogButtonStyle.Destructive => "Destructive",
            _ => ""
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}