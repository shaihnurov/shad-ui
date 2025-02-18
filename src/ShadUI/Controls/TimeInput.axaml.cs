using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;

namespace ShadUI.Controls;

/// <summary>
///     A control that allows users to input and display time values in hours, minutes, and optionally seconds.
///     Supports both 12-hour and 24-hour time formats.
/// </summary>
public class TimeInput : TemplatedControl
{
    /// <summary>
    ///     Defines the <see cref="Value" /> property.
    /// </summary>
    public static readonly StyledProperty<TimeSpan?> ValueProperty =
        AvaloniaProperty.Register<TimeInput, TimeSpan?>(nameof(Value), enableDataValidation: true);

    /// <summary>
    ///     Gets or sets the current time value.
    /// </summary>
    public TimeSpan? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="HourString" /> property.
    /// </summary>
    internal static readonly StyledProperty<string> HourStringProperty =
        AvaloniaProperty.Register<TimeInput, string>(nameof(HourString), "00");

    /// <summary>
    ///     Gets or sets the string representation of the hours.
    /// </summary>
    internal string HourString
    {
        get => GetValue(HourStringProperty);
        set => SetValue(HourStringProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="MinuteString" /> property.
    /// </summary>
    internal static readonly StyledProperty<string> MinuteStringProperty =
        AvaloniaProperty.Register<TimeInput, string>(nameof(MinuteString), "00");

    /// <summary>
    ///     Gets or sets the string representation of the minutes.
    /// </summary>
    internal string MinuteString
    {
        get => GetValue(MinuteStringProperty);
        set => SetValue(MinuteStringProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="SecondString" /> property.
    /// </summary>
    internal static readonly StyledProperty<string> SecondStringProperty =
        AvaloniaProperty.Register<TimeInput, string>(nameof(SecondString), "00");

    /// <summary>
    ///     Gets or sets the string representation of the seconds.
    /// </summary>
    internal string SecondString
    {
        get => GetValue(SecondStringProperty);
        set => SetValue(SecondStringProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="ClockIdentifier" /> property.
    /// </summary>
    public static readonly StyledProperty<string> ClockIdentifierProperty =
        AvaloniaProperty.Register<TimePicker, string>(nameof(ClockIdentifier), "12HourClock", coerce: CoerceClockIdentifier);

    /// <summary>
    ///     Gets or sets the clock format identifier. Valid values are "12HourClock" or "24HourClock".
    /// </summary>
    public string ClockIdentifier
    {
        get => GetValue(ClockIdentifierProperty);
        set => SetValue(ClockIdentifierProperty, value);
    }

    /// <summary>
    ///     Coerces the clock identifier value to ensure it's valid.
    /// </summary>
    /// <param name="sender">The object that is being coerced.</param>
    /// <param name="value">The value to coerce.</param>
    /// <returns>The coerced value.</returns>
    /// <exception cref="ArgumentException">Thrown when the clock identifier is invalid.</exception>
    private static string CoerceClockIdentifier(AvaloniaObject sender, string value)
    {
        if (!(string.IsNullOrEmpty(value) || value == "12HourClock" || value == "24HourClock"))
            throw new ArgumentException("Invalid ClockIdentifier", default(string));

        return value;
    }

    /// <summary>
    ///     Defines the <see cref="UseSeconds" /> property.
    /// </summary>
    public static readonly StyledProperty<bool> UseSecondsProperty =
        AvaloniaProperty.Register<TimeInput, bool>(nameof(UseSeconds));

    /// <summary>
    ///     Gets or sets whether seconds should be displayed and editable.
    /// </summary>
    public bool UseSeconds
    {
        get => GetValue(UseSecondsProperty);
        set => SetValue(UseSecondsProperty, value);
    }

    /// <summary>
    ///     Defines the <see cref="InputFocus" /> property.
    /// </summary>
    internal static readonly StyledProperty<bool> InputFocusProperty =
        AvaloniaProperty.Register<TimeInput, bool>(nameof(InputFocus));

    /// <summary>
    ///     Gets or sets whether any part of the control has input focus.
    /// </summary>
    internal bool InputFocus
    {
        get => GetValue(InputFocusProperty);
        set => SetValue(InputFocusProperty, value);
    }

    private TextBox? _minuteTextBox;
    private TextBox? _secondTextBox;
    private ToggleButton? _toggleButton;

    /// <summary>
    ///     Called when the template is applied to the control.
    ///     Sets up event handlers and initializes the control state.
    /// </summary>
    /// <param name="e">Contains information about the template being applied.</param>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var hourTextBox = e.NameScope.Get<TextBox>("PART_HourTextBox");
        var minuteTextBox = e.NameScope.Get<TextBox>("PART_MinuteTextBox");
        var secondTextBox = e.NameScope.Get<TextBox>("PART_SecondTextBox");
        var toggleButton = e.NameScope.Get<ToggleButton>("PART_ToggleButton");

        _minuteTextBox = minuteTextBox;
        _secondTextBox = secondTextBox;
        _toggleButton = toggleButton;

        if (Value is { Hours: >= 12 })
        {
            toggleButton.IsChecked = true;
            toggleButton.Content = "PM";
        }
        else
        {
            toggleButton.IsChecked = false;
            toggleButton.Content = "AM";
        }

        hourTextBox.LostFocus += OnTextBoxLostFocus;
        minuteTextBox.LostFocus += OnTextBoxLostFocus;
        secondTextBox.LostFocus += OnTextBoxLostFocus;
        toggleButton.LostFocus += (_, _) => InputFocus = false;

        hourTextBox.GotFocus += (_, _) => InputFocus = true;
        minuteTextBox.GotFocus += (_, _) => InputFocus = true;
        secondTextBox.GotFocus += (_, _) => InputFocus = true;
        toggleButton.GotFocus += (_, _) => InputFocus = true;

        hourTextBox.TextChanged += OnInputChanged;
        minuteTextBox.TextChanged += OnInputChanged;
        secondTextBox.TextChanged += OnInputChanged;
        toggleButton.IsCheckedChanged += OnToggleButtonCheckChanged;

        hourTextBox.KeyDown += (_, _) => _fromInput = true;
        minuteTextBox.KeyDown += (_, _) => _fromInput = true;
        secondTextBox.KeyDown += (_, _) => _fromInput = true;

        hourTextBox.KeyUp += (_, _) => _fromInput = false;
        minuteTextBox.KeyUp += (_, _) => _fromInput = false;
        secondTextBox.KeyUp += (_, _) => _fromInput = false;
    }

    private bool _isUpdating;
    private bool _fromInput;

    /// <summary>
    ///     Handles changes to the input text boxes, updating the time value accordingly.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments.</param>
    private void OnInputChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox textBox) return;

        if (_isUpdating || !_fromInput) return;

        _isUpdating = true;
        int.TryParse(textBox.Text, out var value);

        if (textBox.Name == "PART_HourTextBox")
        {
            if (value >= 24) value = 0;

            if (ClockIdentifier == "12HourClock")
                if (_toggleButton is not null && value >= 12)
                {
                    _toggleButton.IsChecked = true;
                    _toggleButton.Content = "PM";
                    HourString = (value - 12).ToString().PadLeft(2, '0');
                }

            Value = new TimeSpan(value, Value?.Minutes ?? 0, Value?.Seconds ?? 0);

            if (_fromInput && textBox.Text?.Length >= 2)
            {
                _minuteTextBox?.Focus();
                _minuteTextBox?.SelectAll();
            }
        }

        if (textBox.Name == "PART_MinuteTextBox")
        {
            if (value >= 60) value = 0;
            Value = new TimeSpan(Value?.Hours ?? 0, value, Value?.Seconds ?? 0);

            if (_fromInput && textBox.Text?.Length >= 2)
            {
                _secondTextBox?.Focus();
                _secondTextBox?.SelectAll();
            }
        }

        if (textBox.Name == "PART_SecondTextBox")
        {
            if (value >= 60) value = 0;
            Value = new TimeSpan(Value?.Hours ?? 0, Value?.Minutes ?? 0, value);
        }

        _isUpdating = false;
    }

    /// <summary>
    ///     Handles the loss of focus from input text boxes, ensuring valid time values.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments.</param>
    private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox) return;

        int.TryParse(textBox.Text, out var value);

        if (textBox.Name == "PART_HourTextBox")
        {
            if (value >= 24) value = 0;

            if (_toggleButton?.IsChecked == false && value == 12) value = 0;
            if (_toggleButton?.IsChecked == true && value > 12) value -= 12;

            if (_toggleButton is not null && value == 0)
            {
                _toggleButton.Content = "AM";
                _toggleButton.IsChecked = false;
            }
        }

        if (textBox.Name == "PART_MinuteTextBox")
            if (value >= 60)
                value = 0;

        if (textBox.Name == "PART_SecondTextBox")
            if (value >= 60)
                value = 0;

        _fromInput = false;
        textBox.Text = value.ToString().PadLeft(2, '0');

        InputFocus = false;
    }

    /// <summary>
    ///     Handles changes to the AM/PM toggle button, updating the time value accordingly.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments.</param>
    private void OnToggleButtonCheckChanged(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleButton toggleButton) return;

        if (_isUpdating) return;

        _isUpdating = true;
        _fromInput = false;
        toggleButton.Content = toggleButton.IsChecked == true ? "PM" : "AM";

        if (toggleButton.IsChecked == true)
        {
            Value ??= new TimeSpan(12, 0, 0);

            if (Value is { Hours: < 12 })
                Value = new TimeSpan(Value.Value.Hours + 12, Value.Value.Minutes, Value.Value.Seconds);

            if (HourString == "00") HourString = "12";
        }
        else
        {
            if (Value is { Hours: >= 12 })
                Value = new TimeSpan(Value.Value.Hours - 12, Value.Value.Minutes, Value.Value.Seconds);

            if (HourString == "12") HourString = "00";
        }

        _isUpdating = false;
    }

    /// <summary>
    ///     Called when a property value changes. Handles updates to the time value and updates the display strings.
    /// </summary>
    /// <param name="change">Information about the property that changed.</param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property != ValueProperty) return;

        var newValue = change.GetNewValue<TimeSpan?>();

        if (_isUpdating) return;

        _isUpdating = true;

        if (newValue.HasValue)
        {
            HourString = newValue.Value.Hours.ToString().PadLeft(2, '0');
            MinuteString = newValue.Value.Minutes.ToString().PadLeft(2, '0');
            SecondString = UseSeconds ? newValue.Value.Seconds.ToString().PadLeft(2, '0') : "00";

            if (!UseSeconds)
            {
                Value = new TimeSpan(newValue.Value.Hours, newValue.Value.Minutes, 0);
                SecondString = "00";
            }
        }
        else
        {
            HourString = "00";
            MinuteString = "00";
            SecondString = "00";
        }

        _isUpdating = false;
    }

    /// <summary>
    ///     Updates the data validation state of the control.
    /// </summary>
    /// <param name="property">The property that is being validated.</param>
    /// <param name="state">The current binding state.</param>
    /// <param name="error">The validation error, if any.</param>
    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);

        if (property == ValueProperty)
            DataValidationErrors.SetError(this, error);
    }
}