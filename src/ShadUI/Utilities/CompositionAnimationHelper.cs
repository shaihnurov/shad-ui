using System;
using Avalonia.Rendering.Composition;

namespace ShadUI.Utilities;

/// <summary>
/// Useful helper for creating composition animations.
/// </summary>
internal class CompositionAnimationHelper
{
    /// <summary>
    /// Makes the opacity of the visual animated.
    /// </summary>
    /// <param name="compositionVisual">The <see cref="CompositionVisual"/> element</param>
    /// <param name="duration">The duration of the animation in milliseconds</param>
    public static void MakeOpacityAnimated(CompositionVisual? compositionVisual, double duration = 700)
    {
        if (compositionVisual == null) return;

        var compositor = compositionVisual.Compositor;

        var animationGroup = compositor.CreateAnimationGroup();

        var opacityAnimation = compositor.CreateScalarKeyFrameAnimation();
        opacityAnimation.Target = "Opacity";
        opacityAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
        opacityAnimation.Duration = TimeSpan.FromMilliseconds(duration);

        var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
        offsetAnimation.Target = "Offset";
        offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
        offsetAnimation.Duration = TimeSpan.FromMilliseconds(duration);

        animationGroup.Add(offsetAnimation);
        animationGroup.Add(opacityAnimation);

        var implicitAnimationCollection = compositor.CreateImplicitAnimationCollection();
        implicitAnimationCollection["Opacity"] = animationGroup;
        implicitAnimationCollection["Offset"] = animationGroup;

        compositionVisual.ImplicitAnimations = implicitAnimationCollection;
    }
}