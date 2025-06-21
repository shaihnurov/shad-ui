using Avalonia;
using Avalonia.Data.Converters;

namespace ShadUI.Converters;

internal static class CornerRadiusConverter
{
    public static readonly IValueConverter TopOnly =
        new FuncValueConverter<CornerRadius, CornerRadius>(x => new CornerRadius(x.TopLeft, x.TopRight, 0, 0));
    public static readonly IValueConverter BottomOnly =
        new FuncValueConverter<CornerRadius, CornerRadius>(x => new CornerRadius(0, 0, x.BottomRight, x.BottomLeft));
    public static readonly IValueConverter LeftOnly =
        new FuncValueConverter<CornerRadius, CornerRadius>(x => new CornerRadius(x.TopLeft, 0, 0, x.BottomLeft));
    public static readonly IValueConverter RightOnly =
        new FuncValueConverter<CornerRadius, CornerRadius>(x => new CornerRadius(0, x.TopRight, x.BottomRight, 0));
    
    public static readonly IValueConverter TopLeft =
        new FuncValueConverter<CornerRadius, double>(x => x.TopLeft);
    
    public static readonly IValueConverter BottomRight =
        new FuncValueConverter<CornerRadius, double>(x => x.BottomRight);
}