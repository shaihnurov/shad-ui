using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;

// ReSharper disable once CheckNamespace
namespace ShadUI;

/// <summary>
/// Provides extension methods for managing window state persistence in Avalonia applications.
/// This class allows windows to automatically save and restore their position, size, and state
/// across application sessions in temporary files.
/// </summary>
public static class WindowExt
{
    private static readonly Dictionary<Window, EventHandler<WindowClosingEventArgs>> Handlers = new();
    private static readonly Dictionary<string, WindowSettings?> Cache = new();
    private static readonly Dictionary<string, CancellationTokenSource> SaveTokens = new();
    private static readonly object CacheLock = new();

    private const string FileExtension = ".txt";
    private const string FilePrefix = "shadui_windowstate_";

    /// <summary>
    /// Enables automatic window state management for the specified window.
    /// The window's position, size, and state will be automatically saved when the window closes
    /// and restored when the window is opened again.
    /// </summary>
    /// <param name="window">The window to manage state for.</param>
    /// <param name="key">A unique identifier for this window's state. Used to distinguish between different windows.
    /// Defaults to "main" if not specified.</param>
    public static void ManageWindowState(this Window window, string key = "main")
    {
        if (Handlers.ContainsKey(window))
        {
            return;
        }

        var file = GetSettingsFilePath(key);

        RestoreWindowState(window, file);

        EventHandler<WindowClosingEventArgs> handler = (_, __) => SaveWindowState(window, file);
        window.Closing += handler;
        Handlers[window] = handler;
    }

    /// <summary>
    /// Disables automatic window state management for the specified window.
    /// Removes the event handler that was previously attached by <see cref="ManageWindowState"/>.
    /// </summary>
    /// <param name="window">The window to stop managing state for.</param>
    public static void UnmanageWindowState(this Window window)
    {
        if (Handlers.TryGetValue(window, out var handler))
        {
            window.Closing -= handler;
            Handlers.Remove(window);
        }
    }

    private static void RestoreWindowState(Window window, string file)
    {
        WindowSettings? state;

        lock (CacheLock)
        {
            if (!Cache.TryGetValue(file, out state))
            {
                if (File.Exists(file))
                {
                    try
                    {
                        state = LoadWindowSettings(file);
                    }
                    catch
                    {
                        //ignore
                    }
                }

                Cache[file] = state;
            }
        }

        if (state == null) return;

        window.Position = new PixelPoint(state.X, state.Y);
        window.Width = state.Width;
        window.Height = state.Height;
        window.WindowState = state.WindowState == WindowState.Minimized
            ? WindowState.Normal
            : state.WindowState;
    }

    private static void SaveWindowState(Window window, string file)
    {
        var current = new WindowSettings
        {
            X = window.Position.X,
            Y = window.Position.Y,
            Width = window.Width,
            Height = window.Height,
            WindowState = window.WindowState
        };

        lock (CacheLock)
        {
            if (Cache.TryGetValue(file, out var previous) && current.Equals(previous))
            {
                return;
            }

            Cache[file] = current;
        }

        // Debounce saves to avoid excessive file I/O
        if (SaveTokens.TryGetValue(file, out var existingToken))
        {
            existingToken.Cancel();
        }

        var cts = new CancellationTokenSource();
        SaveTokens[file] = cts;

        _ = Task.Delay(100, cts.Token).ContinueWith(_ =>
        {
            if (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    SaveWindowSettings(current, file);
                }
                catch
                {
                    //ignore
                }
                finally
                {
                    lock (CacheLock)
                    {
                        SaveTokens.Remove(file);
                    }
                }
            }
        }, cts.Token);
    }

    private static string GetSettingsFilePath(string key)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var keySpan = key.AsSpan();
        var result = new List<char>(key.Length + FilePrefix.Length + FileExtension.Length);

        result.AddRange(FilePrefix);

        foreach (var c in keySpan)
        {
            result.Add(Array.IndexOf(invalidChars, c) >= 0 ? '_' : c);
        }

        result.AddRange(FileExtension);

        return Path.Combine(Path.GetTempPath(), new string(result.ToArray()));
    }

    private static WindowSettings? LoadWindowSettings(string file)
    {
        if (!File.Exists(file))
        {
            return null;
        }

        var settings = new WindowSettings();
        var lines = File.ReadAllLines(file);

        foreach (var line in lines)
        {
            var lineSpan = line.AsSpan();
            var equalsIndex = lineSpan.IndexOf('=');
            if (equalsIndex <= 0 || equalsIndex >= lineSpan.Length - 1) continue;

            var keySpan = lineSpan.Slice(0, equalsIndex).Trim();
            var valueSpan = lineSpan.Slice(equalsIndex + 1).Trim();

            if (keySpan.SequenceEqual("X".AsSpan()) && int.TryParse(valueSpan, out var x))
            {
                settings.X = x;
            }
            else if (keySpan.SequenceEqual("Y".AsSpan()) && int.TryParse(valueSpan, out var y))
            {
                settings.Y = y;
            }
            else if (keySpan.SequenceEqual("Width".AsSpan()) && double.TryParse(valueSpan, out var width))
            {
                settings.Width = width;
            }
            else if (keySpan.SequenceEqual("Height".AsSpan()) && double.TryParse(valueSpan, out var height))
            {
                settings.Height = height;
            }
            else if (keySpan.SequenceEqual("WindowState".AsSpan()) &&
                     Enum.TryParse<WindowState>(valueSpan.ToString(), out var windowState))
            {
                settings.WindowState = windowState;
            }
        }

        return settings;
    }

    private static void SaveWindowSettings(WindowSettings settings, string file)
    {
        var lines = new string[5];
        lines[0] = $"X={settings.X}";
        lines[1] = $"Y={settings.Y}";
        lines[2] = $"Width={settings.Width}";
        lines[3] = $"Height={settings.Height}";
        lines[4] = $"WindowState={settings.WindowState}";

        File.WriteAllLines(file, lines);
    }

    /// <summary>
    /// Represents the state of a window including its position, size, and window state.
    /// This class is used internally to serialize and deserialize window state information
    /// to and from temporary files.
    /// </summary>
    private class WindowSettings
    {
        /// <summary>
        /// Gets or sets the X coordinate of the window's position on screen.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the window's position on screen.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the window in pixels.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the window in pixels.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the current state of the window (Normal, Minimized, Maximized).
        /// </summary>
        public WindowState WindowState { get; set; }
    }
}