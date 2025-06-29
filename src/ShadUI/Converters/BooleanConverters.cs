using Avalonia.Controls;
// ReSharper disable once CheckNamespace
using Avalonia.Data.Converters;

// ReSharper disable once CheckNamespace
namespace ShadUI;

internal static class BooleanConverters
{
    public static readonly IValueConverter ToInverseOpacity =
        new FuncValueConverter<bool, int>(value => value ? 0 : 1);

    public static readonly IValueConverter ToLoading =
        new FuncValueConverter<bool, object>(value => value ? new Loading() : new Panel());
}