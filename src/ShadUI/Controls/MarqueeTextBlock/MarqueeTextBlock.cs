using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;

namespace ShadUI
{
    /// <summary>
    /// Templated control that scrolls its text horizontally when the content
    /// width exceeds the available bounds (marquee / ticker effect).
    /// Requires PART_Canvas and PART_Text named parts in its control template.
    /// </summary>
    public class MarqueeTextBlock : TemplatedControl
    {
        /// <summary>
        /// Defines the <see cref="TextValue"/> styled property
        /// </summary>
        public static readonly StyledProperty<string> TextValueProperty =
            AvaloniaProperty.Register<MarqueeTextBlock, string>(nameof(TextValue), defaultValue: string.Empty);

        /// <summary>
        /// Gets or sets the text content displayed by this control
        /// </summary>
        public string TextValue
        {
            get => GetValue(TextValueProperty);
            set => SetValue(TextValueProperty, value);
        }

        /// <summary>
        /// Scrolling speed in pixels per second.
        /// Duration is derived from this value so speed stays constant for any text length.
        /// </summary>
        private const double ScrollSpeedPxPerSecond = 80.0;

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
        /// Resolves PART_Canvas and PART_Text, mirrors CSS classes, and wires up
        /// all subscriptions that trigger marquee re-evaluation.
        /// Called on first apply and again on theme changes — old handlers are
        /// always unsubscribed first to prevent duplicate firings and memory leaks.
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

            ((INotifyCollectionChanged)Classes).CollectionChanged += _classesChangedHandler;

            AttachedToVisualTree += _attachedHandler;
            SizeChanged += _sizeChangedHandler;

            RequestMarqueeStart();
        }

        /// <summary>
        /// Reports the TextBlock's natural height as the control's desired height.
        /// Canvas always returns DesiredSize = (0, 0), so without this override the
        /// control collapses and becomes invisible unless MinHeight is set explicitly.
        /// <para>
        /// After <c>base.MeasureOverride</c>, Canvas has already measured the TextBlock
        /// with infinite space — no additional Measure call is needed.
        /// </para>
        /// </summary>
        protected override Size MeasureOverride(Size availableSize)
        {
            var baseSize = base.MeasureOverride(availableSize);

            if (_textBlock is null) 
                return baseSize;

            return new Size(baseSize.Width, _textBlock.DesiredSize.Height);
        }


        /// <summary>Pushes the current <see cref="TextValue"/> into the inner TextBlock.</summary>
        private void UpdateTextBlock()
        {
            if (_textBlock is null) 
                return;
            _textBlock.Text = TextValue;
        }

        /// <summary>
        /// Copies non-pseudo CSS classes from this control to the inner TextBlock
        /// so that ShadUI typography classes (e.g. Small, Muted) are applied correctly.
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
        /// Cancels any pending debounce timer and schedules a fresh marquee start
        /// after 150 ms to coalesce rapid layout/property-change events.
        /// </summary>
        private void RequestMarqueeStart()
        {
            _debounceCts?.Cancel();
            _debounceCts?.Dispose();
            _debounceCts = new CancellationTokenSource();
            RunAfterDelay(_debounceCts.Token);
        }

        /// <summary>
        /// Waits for the debounce delay, disposes the completed CTS, then forwards
        /// to <see cref="StartMarquee"/>. Exits silently if cancelled.
        /// </summary>
        private async void RunAfterDelay(CancellationToken debounceToken)
        {
            try
            {
                await Task.Delay(150, debounceToken);

                _debounceCts?.Dispose();
                _debounceCts = null;

                StartMarquee();
            }
            catch (OperationCanceledException) { }
        }

        /// <summary>
        /// Starts a new marquee loop when the text is wider than the control,
        /// or positions the text statically when it fits.
        /// <para>
        /// All guard checks run before the <see cref="CancellationTokenSource"/> is
        /// allocated, so no CTS is ever created for a no-op call.
        /// </para>
        /// </summary>
        private async void StartMarquee()
        {
            if (_textBlock is null || _canvas is null) 
                return;

            if (Bounds.Width <= 0) 
                return;

            var textWidth = _textBlock.DesiredSize.Width;
            if (textWidth <= 0) 
                return;

            Canvas.SetLeft(_textBlock, 0);
            Canvas.SetTop(_textBlock,
                Math.Max(0, (Bounds.Height - _textBlock.DesiredSize.Height) / 2));

            if (textWidth <= Bounds.Width) 
                return;

            _animationCts?.Cancel();
            _animationCts?.Dispose();
            _animationCts = new CancellationTokenSource();
            var token = _animationCts.Token;

            double startX = Bounds.Width;
            double endX = -textWidth;
            double totalDistance = startX - endX;
            var duration = TimeSpan.FromSeconds(totalDistance / ScrollSpeedPxPerSecond);

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
                        Setters = { new Setter(Canvas.LeftProperty, startX) }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1),
                        Setters = { new Setter(Canvas.LeftProperty, endX) }
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
                System.Diagnostics.Debug.WriteLine(
                    $"[MarqueeTextBlock] Unexpected animation error: {ex}");
            }
        }

        /// <summary>
        /// Unsubscribes all event handlers and nulls their references to break
        /// closures and allow GC. Shared by <see cref="OnApplyTemplate"/> (re-apply guard)
        /// and <see cref="OnDetachedFromVisualTree"/> (final cleanup).
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
                ((INotifyCollectionChanged)Classes).CollectionChanged -= _classesChangedHandler;
                _classesChangedHandler = null;
            }
        }

        /// <summary>
        /// Cancels all in-flight timers and animations, releases template part references,
        /// then delegates to <see cref="DetachEventHandlers"/> to release all subscriptions.
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
        /// Reacts to property changes — updates the inner TextBlock text
        /// and requests a marquee restart when <see cref="TextValue"/> changes.
        /// </summary>
        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == TextValueProperty)
            {
                UpdateTextBlock();
                RequestMarqueeStart();
            }
        }
    }
}