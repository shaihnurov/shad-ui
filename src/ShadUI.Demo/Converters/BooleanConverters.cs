using Avalonia.Data.Converters;

namespace ShadUI.Demo.Converters;

public static class BooleanConverters
{
    public static readonly IValueConverter Opaque =
        new FuncValueConverter<bool, int>(value => value ? 1 : 0);
}