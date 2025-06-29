using Avalonia.Controls;
using Avalonia.Data.Converters;

// ReSharper disable once CheckNamespace
namespace ShadUI;

internal static class ColorModelConverters
{
    public static readonly IValueConverter IsRgb =
        new FuncValueConverter<ColorModel, bool>(value => value == ColorModel.Rgba);

    public static readonly IValueConverter IsHsva =
        new FuncValueConverter<ColorModel, bool>(value => value == ColorModel.Hsva);
}