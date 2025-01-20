using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Styling;
using ShadUI.Themes;
using SkiaSharp;

namespace ShadUI.Utilities.Effects;

/// <summary>
///     Represents an SKSL shader that ShadUI can handle and pass relevant uniforms into.
///     Use the static methods <see cref="BaseEffect.FromEmbeddedResource" /> and <see cref="BaseEffect.FromString" /> for
///     creation.
/// </summary>
public class BaseEffect
{
    // Basic uniforms passed into the shader from the CPU.
    private static readonly string[] Uniforms =
    [
        "uniform float iTime;",
        "uniform float iDark;",
        "uniform float iAlpha;",
        "uniform vec3 iResolution;",
        "uniform vec3 iPrimary;",
        "uniform vec3 iAccent;",
        "uniform vec3 iBase;"
    ];

    /// <summary>
    ///     Represents an SKSL shader that ShadUI can handle and pass relevant uniforms into.
    /// </summary>
    // ReSharper disable once CollectionNeverUpdated.Global
    public static readonly List<BaseEffect> LoadedEffects = [];

    private readonly string _rawShaderString;
    private readonly string _shaderString;

    /// <summary>
    ///     The compiled <see cref="SKRuntimeEffect" /> that will actually be used in draw calls.
    /// </summary>
    public SKRuntimeEffect Effect { get; }

    private BaseEffect(string shaderString, string rawShaderString)
    {
        _shaderString = shaderString;
        _rawShaderString = rawShaderString;
        var compiledEffect = SKRuntimeEffect.Create(_shaderString, out var errors);
        Effect = compiledEffect ?? throw new ShaderCompilationException(errors);
    }

    static BaseEffect()
    {
        if (Application.Current?.ApplicationLifetime is IControlledApplicationLifetime controlled)
            controlled.Exit += (_, _) => EnsureDisposed();
    }

    /// <summary>
    ///     Attempts to load and compile a ".sksl" shader file from the assembly.
    ///     You don't need to provide the extension.
    ///     The shader will be pre-compiled
    ///     REMEMBER: For files to be discoverable in the assembly they should be marked as an embedded resource.
    /// </summary>
    /// <param name="shaderName">Name of the shader to load, with or without extension. - MUST BE .sksl</param>
    /// <returns>An instance of a BackgroundShader with the loaded shader.</returns>
    public static BaseEffect FromEmbeddedResource(string shaderName)
    {
        shaderName = shaderName.ToLowerInvariant();
        if (!shaderName.EndsWith(".sksl"))
            shaderName += ".sksl";
        var assembly = Assembly.GetEntryAssembly();
        var resName = assembly!.GetManifestResourceNames()
            .FirstOrDefault(x => x.ToLowerInvariant().Contains(shaderName));
        if (resName is null)
        {
            assembly = Assembly.GetExecutingAssembly();
            resName = assembly.GetManifestResourceNames()
                .FirstOrDefault(x => x.ToLowerInvariant().Contains(shaderName));
        }

        if (resName is null)
            throw new FileNotFoundException(
                $"Unable to find a file with the name \"{shaderName}\" anywhere in the assembly.");
        using var tr = new StreamReader(assembly.GetManifestResourceStream(resName)!);
        return FromString(tr.ReadToEnd());
    }

    /// <summary>
    ///     Attempts to compile an sksl shader from a string.
    ///     The shader will be pre-compiled and any errors will be thrown as an exception.
    ///     REMEMBER: For files to be discoverable in the assembly they should be marked as an embedded resource.
    /// </summary>
    /// <param name="shaderString">The shader code to be compiled.</param>
    /// <returns>An instance of a BackgroundShader with the loaded shader</returns>
    public static BaseEffect FromString(string shaderString)
    {
        var sb = new StringBuilder();
        foreach (var uniform in Uniforms)
            sb.AppendLine(uniform);
        sb.Append(shaderString);
        var withUniforms = sb.ToString();
        return new BaseEffect(withUniforms, shaderString);
    }

    private static bool _disposed;

    /// <summary>
    ///     Necessary to make sure all the unmanaged effects are disposed.
    /// </summary>
    internal static void EnsureDisposed()
    {
        if (_disposed)
            throw new InvalidOperationException(
                "BaseEffects should only be disposed once at the app lifecycle end.");
        _disposed = true;
        foreach (var loaded in LoadedEffects)
            loaded.Effect.Dispose();
        LoadedEffects.Clear();
    }

    /// <summary>
    ///     Returns if the object is equal to another object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEffect effect) return false;
        return effect._shaderString == _shaderString;
    }

    /// <summary>
    ///     Returns the hash code of the shader string.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _shaderString.GetHashCode();

    private static readonly float[] White = [0.95f, 0.95f, 0.95f];
    private readonly float[] _backgroundAlloc = new float[3];
    private readonly float[] _backgroundAccentAlloc = new float[3];
    private readonly float[] _backgroundPrimaryAlloc = new float[3];
    private readonly float[] _boundsAlloc = new float[3];

    internal SKShader ToShaderWithUniforms(float timeSeconds, ThemeVariant activeVariant, Rect bounds,
        float animationScale, float alpha = 1f)
    {
        var theme = ShadTheme.GetInstance();
        if (theme is null) throw new InvalidOperationException("No Theme Instance is available.");

        _boundsAlloc[0] = (float) bounds.Width;
        _boundsAlloc[1] = (float) bounds.Height;

        var inputs = new SKRuntimeEffectUniforms(Effect)
        {
            { "iResolution", _boundsAlloc },
            { "iTime", timeSeconds * animationScale },
            {
                "iBase",
                activeVariant == ThemeVariant.Dark
                    ? _backgroundAlloc
                    : White
            },
            { "iAccent", _backgroundAccentAlloc },
            { "iPrimary", _backgroundPrimaryAlloc },
            { "iDark", activeVariant == ThemeVariant.Dark ? 1f : 0f },
            { "iAlpha", alpha }
        };
        return Effect.ToShader(false, inputs);
    }

    internal SKShader ToShaderWithCustomUniforms(Func<SKRuntimeEffect, SKRuntimeEffectUniforms> uniformFactory, float timeSeconds,
        Rect bounds,
        float animationScale, float alpha = 1f)
    {
        var uniforms = uniformFactory(Effect);
        uniforms.Add("iResolution", new[] { (float) bounds.Width, (float) bounds.Height, 0f });
        uniforms.Add("iTime", timeSeconds * animationScale);
        uniforms.Add("iAlpha", alpha);
        return Effect.ToShader(false, uniforms);
    }

    /// <summary>
    ///     Returns the pure shader string without uniforms.
    /// </summary>
    public override string ToString() => _rawShaderString;

    private class ShaderCompilationException(string message) : Exception(message);
}