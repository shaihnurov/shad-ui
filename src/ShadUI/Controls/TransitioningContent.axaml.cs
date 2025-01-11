using System;
using System.Threading;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.Threading;

namespace ShadUI.Controls;

/// <summary>
///    Allows for a smooth transition between two content elements.
/// </summary>
public class TransitioningContent : TemplatedControl
{
    internal static readonly StyledProperty<object?> FirstBufferProperty =
        AvaloniaProperty.Register<TransitioningContent, object?>(nameof(FirstBuffer));

    internal object? FirstBuffer
    {
        get => GetValue(FirstBufferProperty);
        set => SetValue(FirstBufferProperty, value);
    }

    internal static readonly StyledProperty<object?> SecondBufferProperty =
        AvaloniaProperty.Register<TransitioningContent, object?>(nameof(SecondBuffer));

    internal object? SecondBuffer
    {
        get => GetValue(SecondBufferProperty);
        set => SetValue(SecondBufferProperty, value);
    }

    /// <summary>
    /// The content to transition to.
    /// </summary>
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<TransitioningContent, object?>(nameof(Content));

    /// <summary>
    /// Gets or sets the content to transition to.
    /// </summary>
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    private bool _isFirstBufferActive;

    private ContentPresenter? _firstBuffer;
    private ContentPresenter? _secondBuffer;

    private static readonly Animation FadeIn;
    private static readonly Animation FadeOut;

    private ContentPresenter? To => _isFirstBufferActive ? _firstBuffer : _secondBuffer;
    private ContentPresenter? From => _isFirstBufferActive ? _secondBuffer : _firstBuffer;

    private object? _contentBeforeApplied;

    static TransitioningContent()
    {
        FadeIn = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(400),
            Children =
            {
                new KeyFrame
                {
                    Setters =
                    {
                        new Setter
                        {
                            Property = OpacityProperty,
                            Value = 0d
                        }
                    },
                    Cue = new Cue(0d)
                },
                new KeyFrame
                {
                    Setters =
                    {
                        new Setter
                        {
                            Property = OpacityProperty,
                            Value = 1d
                        }
                    },
                    Cue = new Cue(1d)
                }
            },
            FillMode = FillMode.Forward
        };
        FadeOut = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(400),
            Children =
            {
                new KeyFrame
                {
                    Setters =
                    {
                        new Setter
                        {
                            Property = OpacityProperty,
                            Value = 1d
                        }
                    },
                    Cue = new Cue(0d)
                },
                new KeyFrame
                {
                    Setters =
                    {
                        new Setter
                        {
                            Property = OpacityProperty,
                            Value = 0d
                        }
                    },
                    Cue = new Cue(1d)
                }
            },
            FillMode = FillMode.Forward
        };
        FadeIn.Duration = FadeOut.Duration = TimeSpan.FromMilliseconds(250);
    }

    private CancellationTokenSource _animCancellationToken = new();

    /// <summary>
    /// Called when a property changes.
    /// </summary>
    /// <param name="change"></param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ContentProperty)
            PushContent(change.NewValue);
    }

    /// <summary>
    /// Called when the control template is applied.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (e.NameScope.Get<ContentPresenter>("PART_FirstBufferControl") is { } fBuff)
            _firstBuffer = fBuff;
        if (e.NameScope.Get<ContentPresenter>("PART_SecondBufferControl") is { } sBuff)
            _secondBuffer = sBuff;
        if (_contentBeforeApplied != null)
        {
            PushContent(_contentBeforeApplied);
            _contentBeforeApplied = null;
        }
    }

    private void PushContent(object? content)
    {
        if (To is null || From is null)
        {
            _contentBeforeApplied = content;
            return;
        }

        _animCancellationToken.Cancel();
        _animCancellationToken.Dispose();
        _animCancellationToken = new CancellationTokenSource();

        if (_isFirstBufferActive) SecondBuffer = content;
        else FirstBuffer = content;
        _isFirstBufferActive = !_isFirstBufferActive;
        try
        {
            FadeOut.RunAsync(From, _animCancellationToken.Token).ContinueWith(_ =>
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    From.IsHitTestVisible = false;
                    if (_isFirstBufferActive) SecondBuffer = null;
                    else FirstBuffer = null;
                });
            });
            FadeIn.RunAsync(To, _animCancellationToken.Token).ContinueWith(_ =>
                Dispatcher.UIThread.Invoke(() => To.IsHitTestVisible = true));
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    /// Called when the control is unloaded.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        _animCancellationToken.Dispose();
    }
}