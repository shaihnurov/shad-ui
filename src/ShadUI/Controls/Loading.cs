using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Rendering.Composition;
using ShadUI.Enums;
using ShadUI.Extensions;
using ShadUI.Utilities.Effects;
using SkiaSharp;

namespace ShadUI.Controls;

/// <summary>
///     Loading indicator.
/// </summary>
public class Loading : Control
{
    /// <summary>
    ///     The style of the loading indicator.
    /// </summary>
    public static readonly StyledProperty<LoadingStyle> LoadingStyleProperty =
        AvaloniaProperty.Register<Loading, LoadingStyle>(nameof(LoadingStyle));

    /// <summary>
    ///     Gets or sets the value of the <see cref="LoadingStyleProperty" />.
    /// </summary>
    public LoadingStyle LoadingStyle
    {
        get => GetValue(LoadingStyleProperty);
        set => SetValue(LoadingStyleProperty, value);
    }

    /// <summary>
    ///     The foreground color of the loading indicator.
    /// </summary>
    public static readonly StyledProperty<IBrush?> ForegroundProperty =
        AvaloniaProperty.Register<Loading, IBrush?>(nameof(Foreground));

    /// <summary>
    ///     Gets or sets the value of the <see cref="ForegroundProperty" />.
    /// </summary>
    public IBrush? Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    private static readonly IReadOnlyDictionary<LoadingStyle, BaseEffect> Effects =
        new Dictionary<LoadingStyle, BaseEffect>
        {
            { LoadingStyle.Simple, BaseEffect.FromEmbeddedResource("simple") },
            { LoadingStyle.Glow, BaseEffect.FromEmbeddedResource("glow") },
            { LoadingStyle.Pellets, BaseEffect.FromEmbeddedResource("pellets") }
        };

    private CompositionCustomVisual? _customVisual;

    /// <summary>
    ///     Loading indicator.
    /// </summary>
    public Loading()
    {
        Width = 50;
        Height = 50;
    }

    /// <summary>
    ///     Called when attached to the visual tree.
    /// </summary>
    /// <param name="e">The element</param>
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var comp = ElementComposition.GetElementVisual(this)?.Compositor;
        if (comp == null || _customVisual?.Compositor == comp) return;
        var visualHandler = new LoadingEffectDraw();
        _customVisual = comp.CreateCustomVisual(visualHandler);
        ElementComposition.SetElementChildVisual(this, _customVisual);
        _customVisual.SendHandlerMessage(EffectDrawBase.StartAnimations);

        if (Foreground is null)
            this[!ForegroundProperty] = new DynamicResourceExtension("PrimaryForegroundColor");
        if (Foreground is ImmutableSolidColorBrush brush)
            brush.Color.ToFloatArrayNonAlloc(_color);
        _customVisual.SendHandlerMessage(_color);
        _customVisual.SendHandlerMessage(Effects[LoadingStyle]);
        Update();
    }

    private void Update()
    {
        if (_customVisual == null) return;
        _customVisual.Size = new Vector(Bounds.Width, Bounds.Height);
    }

    private readonly float[] _color = new float[3];

    /// <summary>
    ///     Called when a property changes.
    /// </summary>
    /// <param name="change"></param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == BoundsProperty)
        {
            Update();
        }
        else if (change.Property == ForegroundProperty && Foreground is ImmutableSolidColorBrush brush)
        {
            brush.Color.ToFloatArrayNonAlloc(_color);
            _customVisual?.SendHandlerMessage(_color);
        }
        else if (change.Property == LoadingStyleProperty)
        {
            _customVisual?.SendHandlerMessage(Effects[LoadingStyle]);
        }
    }

    /// <summary>
    ///     The <see cref="EffectDrawBase" /> for the <see cref="Loading" /> indicator.
    /// </summary>
    private class LoadingEffectDraw : EffectDrawBase
    {
        private float[] _color = { 1.0f, 0f, 0f };

        /// <summary>
        ///     The <see cref="LoadingEffectDraw" /> for the <see cref="Loading" /> indicator.
        /// </summary>
        public LoadingEffectDraw()
        {
            AnimationSpeedScale = 2f;
        }

        /// <summary>
        ///     Renders the effect to the canvas.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rect"></param>
        protected override void Render(SKCanvas canvas, SKRect rect)
        {
            using var mainShaderPaint = new SKPaint();

            if (Effect is not null)
            {
                using var shader = EffectWithCustomUniforms(effect => new SKRuntimeEffectUniforms(effect)
                {
                    { "iForeground", _color }
                });
                mainShaderPaint.Shader = shader;
                canvas.DrawRect(rect, mainShaderPaint);
            }
        }

        /// <summary>
        ///     Called when the software renderer is used.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rect"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void RenderSoftware(SKCanvas canvas, SKRect rect)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Called when a message is received.
        /// </summary>
        /// <param name="message"></param>
        public override void OnMessage(object message)
        {
            base.OnMessage(message);
            if (message is float[] color)
                _color = color;
        }
    }
}