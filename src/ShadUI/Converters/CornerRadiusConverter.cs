using Avalonia;
using Avalonia.Data.Converters;

namespace ShadUI.Converters;

internal static class CornerRadiusConverter
{
    public static readonly IValueConverter TopOnly =
        new FuncValueConverter<CornerRadius, CornerRadius>(x => new CornerRadius(x.TopLeft, x.TopRight, 0, 0));
}