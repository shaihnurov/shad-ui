using Avalonia;
using Avalonia.Controls;
using Avalonia.Rendering.Composition;
using Avalonia.Styling;
using ShadUI.Utilities.Effects;

namespace ShadUI.Controls;

public class BaseBackground : Control
{
    public static readonly StyledProperty<string?> ShaderFileProperty =
        AvaloniaProperty.Register<Window, string?>(nameof(ShaderFile));

    /// <summary>
    ///     Specify a filename of an EMBEDDED RESOURCE file of type `.SkSL` with or without extension and it will be loaded and
    ///     displayed.
    ///     This takes priority over the <see cref="ShaderCode" /> property, which in turns takes priority over
    ///     <see cref="Style" />.
    /// </summary>
    public string? ShaderFile
    {
        get => GetValue(ShaderFileProperty);
        set => SetValue(ShaderFileProperty, value);
    }

    public static readonly StyledProperty<bool> ForceSoftwareRenderingProperty =
        AvaloniaProperty.Register<BaseBackground, bool>(nameof(ForceSoftwareRendering));

    public bool ForceSoftwareRendering
    {
        get => GetValue(ForceSoftwareRenderingProperty);
        set => SetValue(ForceSoftwareRenderingProperty, value);
    }

    private CompositionCustomVisual? _customVisual;

    public BaseBackground()
    {
        IsHitTestVisible = false;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var comp = ElementComposition.GetElementVisual(this)?.Compositor;
        if (comp == null || _customVisual?.Compositor == comp) return;
        var visualHandler = new EffectBackgroundDraw();
        _customVisual = comp.CreateCustomVisual(visualHandler);
        ElementComposition.SetElementChildVisual(this, _customVisual);
        HandleBackgroundStyleChanges();
        Update();
    }

    private void Update()
    {
        if (_customVisual == null) return;
        _customVisual.Size = new Vector(Bounds.Width, Bounds.Height);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == BoundsProperty)
            Update();
        else if (change.Property == ForceSoftwareRenderingProperty && change.NewValue is bool forceSoftwareRendering)
            _customVisual?.SendHandlerMessage(forceSoftwareRendering
                ? EffectDrawBase.EnableForceSoftwareRendering
                : EffectDrawBase.DisableForceSoftwareRendering);
        else if (change.Property == ShaderFileProperty)
            HandleBackgroundStyleChanges();
    }

    private void HandleBackgroundStyleChanges()
    {
        if (_customVisual == null || string.IsNullOrEmpty(ShaderFile)) return;

        var effect = BaseEffect.FromEmbeddedResource(ShaderFile!);
        _customVisual?.SendHandlerMessage(effect);
    }
}