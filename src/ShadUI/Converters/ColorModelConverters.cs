using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace ShadUI.Converters;

internal static class ColorModelConverters
{
    public static readonly IValueConverter IsRgb =
        new FuncValueConverter<ColorModel, bool>(value => value == ColorModel.Rgba);

    public static readonly IValueConverter IsHsva =
        new FuncValueConverter<ColorModel, bool>(value => value == ColorModel.Hsva);
}