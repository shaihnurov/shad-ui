using System;
using System.Diagnostics;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.Composition;
using Avalonia.Skia;
using Avalonia.Styling;
using ShadUI.Themes;
using SkiaSharp;

namespace ShadUI.Utilities.Effects;

/// <summary>
/// Effect drawing base class.
/// </summary>
public abstract class EffectDrawBase : CompositionCustomVisualHandler
{
    /// <summary>
    /// Represents the start of animations.
    /// </summary>
    public static readonly object StartAnimations = new();

    /// <summary>
    /// Represents the stop of animations.
    /// </summary>
    public static readonly object StopAnimations = new();

    private BaseEffect? _effect;

    /// <summary>
    /// Returns the current effect.
    /// </summary>
    protected BaseEffect? Effect
    {
        get => _effect;
        private set
        {
            var old = _effect;
            if (Equals(old, value)) return;
            _effect = value;
            EffectChanged(old, _effect);
        }
    }

    private bool _animationEnabled;

    /// <summary>
    /// Gets or sets whether animations are enabled.
    /// </summary>
    public bool AnimationEnabled
    {
        get => _animationEnabled;
        set
        {
            if (value) _animationTick.Start();
            else _animationTick.Stop();
            _animationEnabled = value;
        }
    }

    /// <summary>
    /// Gets or sets whether to force software rendering.
    /// </summary>
    public bool ForceSoftwareRendering { get; set; }

    /// <summary>
    /// The scale of the animation speed.
    /// </summary>
    protected float AnimationSpeedScale { get; set; } = 0.1f;

    /// <summary>
    /// The active theme variant.
    /// </summary>
    protected ThemeVariant ActiveVariant { get; private set; } = ThemeVariant.Light;

    /// <summary>
    /// The seconds elapsed since the start of the animation.
    /// </summary>
    protected float AnimationSeconds => (float) _animationTick.Elapsed.TotalSeconds;

    private readonly Stopwatch _animationTick = new();
    private readonly bool _invalidateRect;

    /// <summary>
    /// Returns a new instance of the <see cref="EffectDrawBase"/> class.
    /// </summary>
    /// <param name="invalidateRect"></param>
    protected EffectDrawBase(bool invalidateRect = true)
    {
        _invalidateRect = invalidateRect;
        var theme = ShadTheme.GetInstance();
        theme.OnBaseThemeChanged += v => ActiveVariant = v;
    }

    /// <summary>
    /// Called whenever the effect is changed.
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public override void OnRender(ImmediateDrawingContext context)
    {
        var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
        if (leaseFeature is null) throw new InvalidOperationException("Unable to lease Skia API");
        using var lease = leaseFeature.Lease();
        var rect = SKRect.Create((float) EffectiveSize.X, (float) EffectiveSize.Y);
        if (lease.GrContext is null ||
            ForceSoftwareRendering) // GrContext is null whenever there is no hardware acceleration available
            RenderSoftware(lease.SkCanvas, rect);
        else
            Render(lease.SkCanvas, rect);
    }

    /// <summary>
    /// Called when a message is received.
    /// </summary>
    /// <param name="message"></param>
    public override void OnMessage(object message)
    {
        if (message == StartAnimations)
        {
            AnimationEnabled = true;
            RegisterForNextAnimationFrameUpdate();
        }
        else if (message == StopAnimations)
        {
            AnimationEnabled = false;
        }
        else if (message is BaseEffect effect)
        {
            Effect = effect;
        }
    }

    /// <summary>
    /// Called every frame to update the animation.
    /// </summary>
    public override void OnAnimationFrameUpdate()
    {
        if (!AnimationEnabled) return;
        if (_invalidateRect)
            Invalidate(GetRenderBounds());
        else
            Invalidate();
        RegisterForNextAnimationFrameUpdate();
    }

    /// <summary>
    ///     Called every frame to render content.
    /// </summary>
    protected abstract void Render(SKCanvas canvas, SKRect rect);

    /// <summary>
    ///     Called every frame whenever the app falls back to software rendering (or <see cref="ForceSoftwareRendering" /> is
    ///     enabled)
    /// </summary>
    protected abstract void RenderSoftware(SKCanvas canvas, SKRect rect);

    /// <summary>
    /// Returns the shader with the effect and uniforms.
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    protected SKShader? EffectWithUniforms(BaseEffect? effect, float alpha = 1f) =>
        effect?.ToShaderWithUniforms(AnimationSeconds, ActiveVariant, GetRenderBounds(), AnimationSpeedScale, alpha);

    /// <summary>
    /// Returns the shader with the effect and custom uniforms.
    /// </summary>
    /// <param name="uniformFactory"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    protected SKShader? EffectWithCustomUniforms(Func<SKRuntimeEffect, SKRuntimeEffectUniforms> uniformFactory, float alpha = 1f) =>
        EffectWithCustomUniforms(Effect, uniformFactory, alpha);

    /// <summary>
    /// Returns the shader with the effect and custom uniforms.
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="uniformFactory"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    protected SKShader? EffectWithCustomUniforms(BaseEffect? effect, Func<SKRuntimeEffect, SKRuntimeEffectUniforms> uniformFactory,
        float alpha = 1f) =>
        effect?.ToShaderWithCustomUniforms(uniformFactory, AnimationSeconds, GetRenderBounds(), AnimationSpeedScale, alpha);

    /// <summary>
    /// Called whenever the effect is changed.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void EffectChanged(BaseEffect? oldValue, BaseEffect? newValue)
    {
    }
}