using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;

namespace ShadUI;

/// <summary>
/// Scrolls text horizontally when its width exceeds the control bounds (marquee effect)
/// Requires PART_Canvas and PART_Text in the control template
/// </summary>
public class MarqueeTextBlock : TemplatedControl
{
    /// <summary>
    /// Defines the <see cref="TextValue"/> property
    /// </summary>
    public static readonly StyledProperty<string> TextValueProperty =
        AvaloniaProperty.Register<MarqueeTextBlock, string>(nameof(TextValue), defaultValue: string.Empty);

    /// <summary>
    /// Gets or sets the text displayed by this control
    /// </summary>
    public string TextValue
    {
        get => GetValue(TextValueProperty);
        set => SetValue(TextValueProperty, value);
    }

    /// <summary>
    /// Scrolling speed in pixels per second
    /// Duration is derived from this so speed stays constant regardless of text length
    /// </summary>
    private const double ScrollSpeedPxPerSecond = 100.0;

    private TextBlock? _textBlock;
    private Canvas? _canvas;

    private EventHandler<VisualTreeAttachmentEventArgs>? _attachedHandler;
    private EventHandler<SizeChangedEventArgs>? _sizeChangedHandler;
    private NotifyCollectionChangedEventHandler? _classesChangedHandler;

    /// <summary>
    /// Cancels the 150 ms debounce delay when a newer request arrives
    /// </summary>
    private CancellationTokenSource? _debounceCts;

    /// <summary>
    /// Cancels the running animation loop when a restart is needed
    /// </summary>
    private CancellationTokenSource? _animationCts;

    /// <summary>
    /// Resolves template parts, mirrors CSS classes, and wires up event subscriptions
    /// Unsubscribes previous handlers first to handle theme re-application safely
    /// </summary>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachEventHandlers();

        _textBlock = e.NameScope.Find<TextBlock>("PART_Text");
        _canvas = e.NameScope.Find<Canvas>("PART_Canvas");

        UpdateTextBlock();
        UpdateClasses();

        _classesChangedHandler = (_, _) => UpdateClasses();
        _attachedHandler = (_, _) => RequestMarqueeStart();
        _sizeChangedHandler = (_, _) => RequestMarqueeStart();

        Classes.CollectionChanged += _classesChangedHandler;
        AttachedToVisualTree += _attachedHandler;
        SizeChanged += _sizeChangedHandler;

        RequestMarqueeStart();
    }

    /// <summary>
    /// Returns the TextBlock's natural height as the control's desired height
    /// Needed because Canvas always reports (0, 0) and would otherwise collapse the control
    /// </summary>
    protected override Size MeasureOverride(Size availableSize)
    {
        var baseSize = base.MeasureOverride(availableSize);

        if (_textBlock is null) 
            return baseSize;

        return new Size(baseSize.Width, _textBlock.DesiredSize.Height);
    }

    /// <summary>
    /// Syncs <see cref="TextValue"/> into the inner TextBlock
    /// </summary>
    private void UpdateTextBlock()
    {
        if (_textBlock is null) 
            return;

        _textBlock.Text = TextValue ?? string.Empty;
    }

    /// <summary>
    /// Copies non-pseudo CSS classes to the inner TextBlock
    /// so ShadUI typography classes (Small, Muted) are applied correctly
    /// </summary>
    private void UpdateClasses()
    {
        if (_textBlock is null) 
            return;

        _textBlock.Classes.Clear();

        foreach (var c in Classes)
            if (!c.StartsWith(':'))
                _textBlock.Classes.Add(c);
    }

    /// <summary>
    /// Cancels any pending debounce and schedules a new marquee start after 150 ms
    /// Coalesces rapid layout and property-change events into a single restart
    /// </summary>
    private void RequestMarqueeStart()
    {
        if (_canvas is null) 
            return;

        _debounceCts?.Cancel();
        _debounceCts?.Dispose();
        _debounceCts = new CancellationTokenSource();

        RunAfterDelay(_debounceCts.Token);
    }

    /// <summary>
    /// Waits out the debounce delay, then calls <see cref="StartMarquee"/>
    /// Exits silently if cancelled by a newer request
    /// </summary>
    private async void RunAfterDelay(CancellationToken debounceToken)
    {
        try
        {
            await Task.Delay(150, debounceToken);

            var cts = _debounceCts;
            _debounceCts = null;
            cts?.Dispose();

            StartMarquee();
        }
        catch (OperationCanceledException) { }
    }

    /// <summary>
    /// Stops the current animation, then starts a new marquee loop if the text
    /// overflows the control. Shows text statically if it fits
    /// </summary>
    private async void StartMarquee()
    {
        if (_textBlock is null || _canvas is null) 
            return;

        _animationCts?.Cancel();
        _animationCts?.Dispose();
        _animationCts = null;

        if (!IsVisible || Bounds.Width <= 0) 
            return;

        var textWidth = _textBlock.DesiredSize.Width;

        if (textWidth <= 0) 
            return;

        Canvas.SetLeft(_textBlock, 0);
        Canvas.SetTop(_textBlock, Math.Max(0, (Bounds.Height - _textBlock.DesiredSize.Height) / 2));

        if (textWidth <= Bounds.Width) 
            return;

        _animationCts = new CancellationTokenSource();
        var token = _animationCts.Token;

        double startX = Bounds.Width;
        double endX = -textWidth;
        var duration = TimeSpan.FromSeconds((startX - endX) / ScrollSpeedPxPerSecond);

        var animation = new Animation
        {
            Duration = duration,
            IterationCount = new IterationCount(1),
            Easing = new LinearEasing(),
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters =
                    {
                        new Setter(Canvas.LeftProperty, startX)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters =
                    {
                        new Setter(Canvas.LeftProperty, endX)
                    }
                }
            }
        };

        try
        {
            while (!token.IsCancellationRequested)
            {
                Canvas.SetLeft(_textBlock, startX);
                await animation.RunAsync(_textBlock, token);
                await Task.Delay(TimeSpan.FromSeconds(2), token);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            Debug.WriteLine($"[MarqueeTextBlock] Unexpected animation error: {ex}");
        }
        finally
        {
            if (_animationCts?.Token == token)
            {
                _animationCts.Dispose();
                _animationCts = null;
            }
        }
    }

    /// <summary>
    /// Unsubscribes and nulls all event handlers
    /// Called on template re-application and on detach from the visual tree
    /// </summary>
    private void DetachEventHandlers()
    {
        if (_attachedHandler != null)
        {
            AttachedToVisualTree -= _attachedHandler;
            _attachedHandler = null;
        }

        if (_sizeChangedHandler != null)
        {
            SizeChanged -= _sizeChangedHandler;
            _sizeChangedHandler = null;
        }

        if (_classesChangedHandler != null)
        {
            Classes.CollectionChanged -= _classesChangedHandler;
            _classesChangedHandler = null;
        }
    }

    /// <summary>
    /// Cancels all timers and animations, releases template part references, and unsubscribes all event handlers
    /// </summary>
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _debounceCts?.Cancel();
        _debounceCts?.Dispose();
        _debounceCts = null;

        _animationCts?.Cancel();
        _animationCts?.Dispose();
        _animationCts = null;

        _textBlock = null;
        _canvas = null;

        DetachEventHandlers();
    }

    /// <summary>
    /// Handles <see cref="TextValueProperty"/> changes by refreshing text and restarting the marquee,
    /// and <see cref="Visual.IsVisibleProperty"/> changes by restarting or stopping it
    /// </summary>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == TextValueProperty)
        {
            UpdateTextBlock();
            RequestMarqueeStart();
        }
        else if (change.Property == IsVisibleProperty)
            RequestMarqueeStart();
    }
}