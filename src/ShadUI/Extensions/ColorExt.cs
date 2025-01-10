using Avalonia.Media;

namespace ShadUI.Extensions;

/// <summary>
/// Extensions for the <see cref="Color"/> class.
/// </summary>
public static class ColorExt
{
    /// <summary>
    ///     Converts a standard <see cref="Color"/> to an RGB float array in the range 0-1.
    ///     Allows recycling of an array for performance.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="array"></param>
    public static void ToFloatArrayNonAlloc(this Color color, float[] array)
    {
        array[0] = color.R / 255f;
        array[1] = color.G / 255f;
        array[2] = color.B / 255f;
    }
}