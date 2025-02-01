using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace ShadUI.Dialogs;

internal class SimpleDialog : TemplatedControl
{
    private readonly DialogManager? _manager;

    public SimpleDialog()
    {
    }

    public SimpleDialog(DialogManager manager)
    {
        _manager = manager;
    }

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<SimpleDialog, string>(nameof(Title));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<string> MessageProperty =
        AvaloniaProperty.Register<SimpleDialog, string>(nameof(Message));

    public string Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly StyledProperty<string> PrimaryButtonTextProperty =
        AvaloniaProperty.Register<SimpleDialog, string>(nameof(PrimaryButtonText));

    public string PrimaryButtonText
    {
        get => GetValue(PrimaryButtonTextProperty);
        set => SetValue(PrimaryButtonTextProperty, value);
    }

    public static readonly StyledProperty<DialogButtonStyle> PrimaryButtonStyleProperty =
        AvaloniaProperty.Register<SimpleDialog, DialogButtonStyle>(nameof(PrimaryButtonStyle));

    public DialogButtonStyle PrimaryButtonStyle
    {
        get => GetValue(PrimaryButtonStyleProperty);
        set => SetValue(PrimaryButtonStyleProperty, value);
    }

    public Action? PrimaryCallback { get; set; }

    public Func<Task>? PrimaryCallbackAsync { get; set; }

    public static readonly StyledProperty<string> SecondaryButtonTextProperty =
        AvaloniaProperty.Register<SimpleDialog, string>(nameof(SecondaryButtonText));

    public string SecondaryButtonText
    {
        get => GetValue(SecondaryButtonTextProperty);
        set => SetValue(SecondaryButtonTextProperty, value);
    }

    public static readonly StyledProperty<DialogButtonStyle> SecondaryButtonStyleProperty =
        AvaloniaProperty.Register<SimpleDialog, DialogButtonStyle>(nameof(SecondaryButtonStyle), DialogButtonStyle.Secondary);

    public DialogButtonStyle SecondaryButtonStyle
    {
        get => GetValue(SecondaryButtonStyleProperty);
        set => SetValue(SecondaryButtonStyleProperty, value);
    }

    public Action? SecondaryCallback { get; set; }

    public Func<Task>? SecondaryCallbackAsync { get; set; }

    public static readonly StyledProperty<string> TertiaryButtonTextProperty =
        AvaloniaProperty.Register<SimpleDialog, string>(nameof(TertiaryButtonText));

    public string TertiaryButtonText
    {
        get => GetValue(TertiaryButtonTextProperty);
        set => SetValue(TertiaryButtonTextProperty, value);
    }

    public static readonly StyledProperty<DialogButtonStyle> TertiaryButtonStyleProperty =
        AvaloniaProperty.Register<SimpleDialog, DialogButtonStyle>(nameof(TertiaryButtonStyle), DialogButtonStyle.Outline);

    public DialogButtonStyle TertiaryButtonStyle
    {
        get => GetValue(TertiaryButtonStyleProperty);
        set => SetValue(TertiaryButtonStyleProperty, value);
    }

    public Action? TertiaryCallback { get; set; }

    public Func<Task>? TertiaryCallbackAsync { get; set; }

    public static readonly StyledProperty<string> CancelButtonTextProperty =
        AvaloniaProperty.Register<SimpleDialog, string>(nameof(CancelButtonText));

    public string CancelButtonText
    {
        get => GetValue(CancelButtonTextProperty);
        set => SetValue(CancelButtonTextProperty, value);
    }

    public Action? CancelCallback { get; set; }

    public Func<Task>? CancelCallbackAsync { get; set; }

    public static readonly StyledProperty<DialogButtonStyle> CancelButtonStyleProperty =
        AvaloniaProperty.Register<SimpleDialog, DialogButtonStyle>(nameof(CancelButtonStyle), DialogButtonStyle.Outline);

    public DialogButtonStyle CancelButtonStyle
    {
        get => GetValue(CancelButtonStyleProperty);
        set => SetValue(CancelButtonStyleProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        e.NameScope.Get<Button>("PART_PrimaryButton").Click += (_, _) =>
        {
            _manager?.Close(this);
            PrimaryCallback?.Invoke();
            PrimaryCallbackAsync?.Invoke();
        };
        e.NameScope.Get<Button>("PART_SecondaryButton").Click += (_, _) =>
        {
            _manager?.Close(this);
            SecondaryCallback?.Invoke();
            SecondaryCallbackAsync?.Invoke();
        };
        e.NameScope.Get<Button>("PART_TertiaryButton").Click += (_, _) =>
        {
            _manager?.Close(this);
            TertiaryCallback?.Invoke();
            TertiaryCallbackAsync?.Invoke();
        };
        e.NameScope.Get<Button>("PART_CancelButton").Click += (_, _) =>
        {
            _manager?.Close(this);
            CancelCallback?.Invoke();
            CancelCallbackAsync?.Invoke();
        };
    }
}