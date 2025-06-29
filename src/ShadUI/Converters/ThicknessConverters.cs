using System;
using Avalonia;
using Avalonia.Data.Converters;

// ReSharper disable once CheckNamespace
namespace ShadUI;

internal static class ThicknessConverters
{
    public static readonly IValueConverter ToLargest =
        new FuncValueConverter<Thickness, double>(x => Math.Max(x.Left, Math.Max(x.Top, Math.Max(x.Right, x.Bottom))));

    public static readonly IValueConverter NoBottom =
        new FuncValueConverter<Thickness, Thickness>(x => new Thickness(x.Left, x.Top, x.Right, 0));

    public static readonly IValueConverter NoTop =
        new FuncValueConverter<Thickness, Thickness>(x => new Thickness(x.Left, 0, x.Right, x.Bottom));

    public static readonly IValueConverter LeftRight =
        new FuncValueConverter<Thickness, Thickness>(x => new Thickness(x.Left, 0, x.Right, 0));
}