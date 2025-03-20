using Avalonia.Controls;
using Avalonia.Data.Converters;
using ShadUI.Controls;

namespace ShadUI.Converters;

internal static class BooleanConverters
{
    public static readonly IValueConverter ToLoading =
        new FuncValueConverter<bool, object>((value) => value ? new Loading() : new Panel());
}