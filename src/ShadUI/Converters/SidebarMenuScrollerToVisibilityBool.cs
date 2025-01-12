using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ShadUI.Converters;

/// <summary>
///     Converts the value of the sidebar menu scroller to a visibility boolean.
/// </summary>
public class SidebarMenuScrollerToVisibilityBool : IMultiValueConverter
{
    /// <summary>
    ///     Returns the up instance of the <see cref="SidebarMenuScrollerToVisibilityBool" />.
    /// </summary>
    public static SidebarMenuScrollerToVisibilityBool Up { get; } = new((x, y) => x > y);

    /// <summary>
    ///     Returns the down instance of the <see cref="SidebarMenuScrollerToVisibilityBool" />.
    /// </summary>
    public static SidebarMenuScrollerToVisibilityBool Down { get; } = new((x, y) => x < y);

    private readonly Func<double, double, bool> _converter;

    /// <summary>
    ///     Returns the instance of the <see cref="SidebarMenuScrollerToVisibilityBool" />.
    /// </summary>
    /// <param name="converter"></param>
    public SidebarMenuScrollerToVisibilityBool(Func<double, double, bool> converter)
    {
        _converter = converter;
    }

    /// <summary>
    ///     Converts the value of the sidebar menu scroller to a visibility boolean.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is not double firstVal) return null;
        if (values[1] is not double secondVal) return null;
        return _converter(firstVal, secondVal);
    }
}