using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Styling;
using ShadUI.Themes;
using SkiaSharp;

namespace ShadUI.Utilities.Effects;

public abstract class EffectDrawBase : CompositionCustomVisualHandler
{
    public static readonly object StartAnimations = new(),
        StopAnimations = new(),
        EnableForceSoftwareRendering = new(),
        DisableForceSoftwareRendering = new();

    private BaseEffect? _effect;

    public BaseEffect? Effect
    {
        get => _effect;
        set
        {
            var old = _effect;
            if (Equals(old, value)) return;
            _effect = value;
            EffectChanged(old, _effect);
        }
    }

    private bool _animationEnabled;

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

    public bool ForceSoftwareRendering { get; set; }

    protected float AnimationSpeedScale { get; set; } = 0.1f;

    protected ThemeVariant ActiveVariant { get; private set; }

    protected ColorTheme ActiveTheme { get; private set; }

    protected float AnimationSeconds => (float) _animationTick.Elapsed.TotalSeconds;

    private readonly Stopwatch _animationTick = new();
    private readonly bool _invalidateRect;

    protected EffectDrawBase(bool invalidateRect = true)
    {
        _invalidateRect = invalidateRect;
        var theme = BaseTheme.GetInstance();
        theme.OnBaseThemeChanged += v => ActiveVariant = v;
        ActiveVariant = theme.ActiveBaseTheme;
        theme.OnColorThemeChanged += t => ActiveTheme = t;
        ActiveTheme = theme.ActiveColorTheme!;
    }

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

    protected SKShader? EffectWithUniforms(float alpha = 1f) =>
        EffectWithUniforms(Effect, alpha);

    protected SKShader? EffectWithUniforms(BaseEffect? effect, float alpha = 1f) =>
        effect?.ToShaderWithUniforms(AnimationSeconds, ActiveVariant, GetRenderBounds(), AnimationSpeedScale, alpha);

    protected SKShader? EffectWithCustomUniforms(Func<SKRuntimeEffect, SKRuntimeEffectUniforms> uniformFactory, float alpha = 1f) =>
        EffectWithCustomUniforms(Effect, uniformFactory, alpha);

    protected SKShader? EffectWithCustomUniforms(BaseEffect? effect, Func<SKRuntimeEffect, SKRuntimeEffectUniforms> uniformFactory,
        float alpha = 1f) =>
        effect?.ToShaderWithCustomUniforms(uniformFactory, AnimationSeconds, GetRenderBounds(), AnimationSpeedScale, alpha);

    protected virtual void EffectChanged(BaseEffect? oldValue, BaseEffect? newValue)
    {
    }

    public virtual void Dispose()
    {
    }

    public virtual bool Equals(ICustomDrawOperation other) => false;

    public virtual bool HitTest(Point p) => false;
}