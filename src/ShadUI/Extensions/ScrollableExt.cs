using System;
using Avalonia.Rendering.Composition;

namespace ShadUI.Extensions;

/// <summary>
///     Usable extension methods for making an element scrollable.
/// </summary>
public static class ScrollableExt
{
    /// <summary>
    ///     Makes the visual scrollable.
    /// </summary>
    /// <param name="compositionVisual"></param>
    public static void MakeScrollable(CompositionVisual? compositionVisual)
    {
        if (compositionVisual == null)
            return;

        var compositor = compositionVisual.Compositor;

        var animationGroup = compositor.CreateAnimationGroup();
        var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
        offsetAnimation.Target = "Offset";

        offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
        offsetAnimation.Duration = TimeSpan.FromMilliseconds(250);

        var implicitAnimationCollection = compositor.CreateImplicitAnimationCollection();
        animationGroup.Add(offsetAnimation);
        implicitAnimationCollection["Offset"] = animationGroup;
        compositionVisual.ImplicitAnimations = implicitAnimationCollection;
    }
}