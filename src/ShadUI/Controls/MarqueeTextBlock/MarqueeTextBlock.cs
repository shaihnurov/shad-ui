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
    /// A templated control that scrolls its text horizontally when the content
    /// width exceeds the available bounds (marquee / ticker effect).
    /// Requires PART_Canvas and PART_Text named parts in its control template.
    /// </summary>
    public class MarqueeTextBlock : TemplatedControl
    {
        /// <summary>
        /// Defines the <see cref="TextValue"/> styled property
        /// </summary>
        public static readonly StyledProperty<string> TextValueProperty =
            AvaloniaProperty.Register<MarqueeTextBlock, string>(nameof(TextValue));

        /// <summary>
        /// Gets or sets the text content displayed by this control
        /// </summary>
        public string TextValue
        {
            get => GetValue(TextValueProperty);
            set => SetValue(TextValueProperty, value);
        }

        private TextBlock? _textBlock;
        private Canvas? _canvas;

        private Action? _requestMarqueeStart;
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
        /// all subscriptions that trigger marquee re-evaluation
        /// </summary>
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _textBlock = e.NameScope.Find<TextBlock>("PART_Text");
            _canvas = e.NameScope.Find<Canvas>("PART_Canvas");

            UpdateTextBlock();
            UpdateClasses();

            _requestMarqueeStart = RequestMarqueeStart;

            _classesChangedHandler = (_, _) => UpdateClasses();
            if (Classes is INotifyCollectionChanged incc)
                incc.CollectionChanged += _classesChangedHandler;

            _attachedHandler = (_, _) => _requestMarqueeStart();
            _sizeChangedHandler = (_, _) => _requestMarqueeStart();
            this.AttachedToVisualTree += _attachedHandler;
            this.SizeChanged += _sizeChangedHandler;

            _requestMarqueeStart();
        }


        /// <summary>
        /// Pushes the current <see cref="TextValue"/> into the inner TextBlock
        /// </summary>
        private void UpdateTextBlock()
        {
            if (_textBlock is null) return;
            _textBlock.Text = TextValue;
        }

        /// <summary>
        /// Copies non-pseudo CSS classes from this control to the inner TextBlock
        /// so that ShadUI typography classes (e.g. Small, Muted) are applied correctly.
        /// </summary>
        private void UpdateClasses()
        {
            if (_textBlock is null) return;

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
        /// Waits for the debounce delay then forwards to <see cref="StartMarquee"/>.
        /// Exits silently if cancelled before the delay completes.
        /// </summary>
        private async void RunAfterDelay(CancellationToken debounceToken)
        {
            try
            {
                await Task.Delay(150, debounceToken);
                StartMarquee();
            }
            catch (OperationCanceledException) { }
        }

        /// <summary>
        /// Cancels any running animation, then starts a new marquee loop when the
        /// text is wider than the control. Resets the TextBlock position when not scrolling.
        /// The TextBlock is vertically centred inside the Canvas at startup.
        /// </summary>
        private async void StartMarquee()
        {
            if (_textBlock is null || _canvas is null) return;

            _animationCts?.Cancel();
            _animationCts?.Dispose();
            _animationCts = new CancellationTokenSource();
            var token = _animationCts.Token;

            if (Bounds.Width <= 0) return;

            var textWidth = _textBlock.Bounds.Width > 0
                ? _textBlock.Bounds.Width
                : _textBlock.DesiredSize.Width;

            if (textWidth <= 0)
            {
                RequestMarqueeStart();
                return;
            }

            _canvas.Width = Bounds.Width;
            _canvas.Height = Bounds.Height;

            Canvas.SetLeft(_textBlock, 0);
            Canvas.SetTop(_textBlock,
                Math.Max(0, (Bounds.Height - _textBlock.Bounds.Height) / 2));

            if (textWidth <= Bounds.Width) return;

            double startX = Bounds.Width;
            double endX = -textWidth;

            var animation = new Animation
            {
                Duration = TimeSpan.FromSeconds(8),
                IterationCount = new IterationCount(1),
                Easing = new LinearEasing(),
                Children =
                {
                    new KeyFrame
                    {
                        Cue     = new Cue(0),
                        Setters = { new Setter(Canvas.LeftProperty, startX) }
                    },
                    new KeyFrame
                    {
                        Cue     = new Cue(1),
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
        }

        /// <summary>
        /// Cancels all in-flight timers and animations, then disposes every
        /// subscription and event handler to prevent memory leaks.
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

            if (_attachedHandler != null)
            {
                this.AttachedToVisualTree -= _attachedHandler;
                _attachedHandler = null;
            }

            if (_sizeChangedHandler != null)
            {
                this.SizeChanged -= _sizeChangedHandler;
                _sizeChangedHandler = null;
            }

            if (_classesChangedHandler != null && Classes is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged -= _classesChangedHandler;
                _classesChangedHandler = null;
            }
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
                _requestMarqueeStart?.Invoke();
            }
        }
    }
}