using System;
using Avalonia;
using Avalonia.Data.Converters;

namespace ShadUI.Converters;

internal static class ThicknessConverters
{
    public static readonly IValueConverter ToLargest =
        new FuncValueConverter<Thickness, double>(x => Math.Max(x.Left, Math.Max(x.Top, Math.Max(x.Right, x.Bottom))));
}