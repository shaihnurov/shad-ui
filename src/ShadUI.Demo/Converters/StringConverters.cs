using Avalonia.Data.Converters;

namespace ShadUI.Demo.Converters;

public static class StringConverters
{
    public static readonly IValueConverter IsEqual =
        new FuncValueConverter<string, string, bool>((x, y) => x!.Equals(y));
}