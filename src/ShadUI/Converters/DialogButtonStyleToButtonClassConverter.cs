using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ShadUI.Dialogs;

namespace ShadUI.Converters;

internal class DialogButtonStyleToButtonClassConverter : IValueConverter
{
    public static readonly DialogButtonStyleToButtonClassConverter Instance = new();
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SimpleDialogButtonStyle style) return "";

        return style switch
        {
            SimpleDialogButtonStyle.Primary => "Primary",
            SimpleDialogButtonStyle.Secondary => "Secondary",
            SimpleDialogButtonStyle.Outline => "Outline",
            SimpleDialogButtonStyle.Ghost => "Ghost",
            SimpleDialogButtonStyle.Destructive => "Destructive",
            _ => ""
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}