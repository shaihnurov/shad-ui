using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Rendering.Composition;
using ShadUI.Utilities;

namespace ShadUI.Controls;

public class BusyArea : ContentControl
{
    public static readonly StyledProperty<double> LoadingSizeProperty =
        AvaloniaProperty.Register<BusyArea, double>(nameof(LoadingSize));

    public double LoadingSize
    {
        get => GetValue(LoadingSizeProperty);
        set => SetValue(LoadingSizeProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsBusyProperty =
        AvaloniaProperty.Register<BusyArea, bool>(nameof(IsBusy));

    public bool IsBusy
    {
        get => GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (e.NameScope.Find<Border>("PART_AreaBackground") is { } background)
            background.Loaded += (_, _) =>
            {
                var element = ElementComposition.GetElementVisual(background);
                if (element != null)
                    CompositionAnimationHelper.MakeOpacityAnimated(element, 400);
            };
    }
}