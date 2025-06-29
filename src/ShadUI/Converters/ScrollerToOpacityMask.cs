using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

// ReSharper disable once CheckNamespace
namespace ShadUI;

/// <summary>
///     Converts the value of the sidebar menu scroller to an opacity mask.
/// </summary>
internal class ScrollerToOpacityMask : IMultiValueConverter
{
    private readonly Func<double, double, IBrush?> _func;

    /// <summary>
    ///     Top mask.
    /// </summary>
    public static ScrollerToOpacityMask Top { get; } = new((x, y) => x > y ? TopBrush : Brushes.White);

    /// <summary>
    ///     Bottom mask.
    /// </summary>
    public static ScrollerToOpacityMask Bottom { get; } = new((x, y) => x < y ? BottomBrush : Brushes.White);

    private static readonly LinearGradientBrush BottomBrush = new()
    {
        StartPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
        EndPoint = new RelativePoint(0.5, 0.95, RelativeUnit.Relative),
        GradientStops =
        [
            new GradientStop(Colors.Black, 0.9),
            new GradientStop(Colors.Transparent, 1)
        ]
    };

    private static readonly LinearGradientBrush TopBrush = new()
    {
        StartPoint = new RelativePoint(0.5, 1, RelativeUnit.Relative),
        EndPoint = new RelativePoint(0.5, 0.05, RelativeUnit.Relative),
        GradientStops =
        [
            new GradientStop(Colors.Black, 0.9),
            new GradientStop(Colors.Transparent, 1)
        ]
    };

    /// <summary>
    ///     Returns the instance of the <see cref="ScrollerToOpacityMask" />.
    /// </summary>
    /// <param name="func"></param>
    public ScrollerToOpacityMask(Func<double, double, IBrush?> func)
    {
        _func = func;
    }

    /// <summary>
    ///     Converts the value of the scroller to an opacity mask.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count != 2) return null;
        if (values[0] is not double valOne) return null;
        if (values[1] is not double valTwo) return null;
        return _func(valOne, valTwo);
    }
}