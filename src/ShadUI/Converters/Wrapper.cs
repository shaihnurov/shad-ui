using Avalonia.Controls.Converters;
using Avalonia.Controls.Primitives.Converters;

// ReSharper disable once CheckNamespace
namespace ShadUI;

internal static class Wrapper
{
    public static EnumToBoolConverter EnumToBoolConverter { get; } = new();
    public static ToBrushConverter ToBrushConverter { get; } = new();
    public static DoNothingForNullConverter DoNothingForNullConverter { get; } = new();
    public static ColorToDisplayNameConverter ColorToDisplayNameConverter { get; } = new();
    
    public static ContrastBrushConverter ContrastBrushConverter { get; } = new();
    
    public static AccentColorConverter AccentColorConverter { get; } = new();
}