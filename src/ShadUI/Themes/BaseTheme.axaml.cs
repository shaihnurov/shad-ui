using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using ShadUI.Enums;
using ShadUI.Extensions;

namespace ShadUI.Themes;

public class BaseTheme : Styles
{
    public static readonly StyledProperty<BaseColor> ThemeColorProperty =
        AvaloniaProperty.Register<BaseTheme, BaseColor>(nameof(Color), defaultBindingMode: BindingMode.OneTime,
            defaultValue: BaseColor.Blue);

    public static readonly StyledProperty<bool> IsRightToLeftProperty =
        AvaloniaProperty.Register<BaseTheme, bool>(nameof(IsRightToLeft), defaultBindingMode: BindingMode.OneTime,
            defaultValue: false);

    /// <summary>
    ///     Used to assign the ColorTheme at launch,
    /// </summary>
    public BaseColor ThemeColor
    {
        get => GetValue(ThemeColorProperty);
        set
        {
            SetValue(ThemeColorProperty, value);
            SetColorThemeResourcesOnColorThemeChanged();
        }
    }

    public bool IsRightToLeft
    {
        get => GetValue(IsRightToLeftProperty);
        set => SetValue(IsRightToLeftProperty, value);
    }

    /// <summary>
    ///     Called whenever the application's <see cref="ColorTheme" /> is changed.
    ///     Useful where controls cannot use "DynamicResource"
    /// </summary>
    public Action<ColorTheme>? OnColorThemeChanged { get; set; }

    /// <summary>
    ///     Called whenever the application's <see cref="ThemeVariant" /> is changed.
    ///     Useful where controls need to change based on light/dark.
    /// </summary>
    public Action<ThemeVariant>? OnBaseThemeChanged { get; set; }

    /// <summary>
    ///     Currently active <see cref="ColorTheme" />
    ///     If you want to change this please use <see cref="ChangeColorTheme(ColorTheme)" />
    /// </summary>
    public ColorTheme? ActiveColorTheme { get; private set; }

    /// <summary>
    ///     All available Color Themes.
    /// </summary>
    public IAvaloniaReadOnlyList<ColorTheme> ColorThemes => _allThemes;

    /// <summary>
    ///     Currently active <see cref="ThemeVariant" />
    ///     If you want to change this please use <see cref="ChangeBaseTheme" /> or <see cref="SwitchBaseTheme" />
    /// </summary>
    public ThemeVariant ActiveBaseTheme => _app.ActualThemeVariant;

    private readonly Application _app;

    private readonly HashSet<ColorTheme> _colorThemeHashset = [];
    private readonly AvaloniaList<ColorTheme> _allThemes = [];

    public BaseTheme()
    {
        AvaloniaXamlLoader.Load(this);
        _app = Application.Current!;
        _app.ActualThemeVariantChanged += (_, e) => OnBaseThemeChanged?.Invoke(_app.ActualThemeVariant);
        foreach (var theme in DefaultColorThemes)
            AddColorTheme(theme.Value);

        UpdateFlowDirectionResources(IsRightToLeft);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == IsRightToLeftProperty) UpdateFlowDirectionResources(change.GetNewValue<bool>());

        base.OnPropertyChanged(change);
    }

    /// <summary>
    ///     Change the theme to one of the default themes.
    /// </summary>
    /// <param name="color">The <see cref="BaseColor" /> to change to.</param>
    public void ChangeColorTheme(BaseColor color) =>
        ThemeColor = color;

    /// <summary>
    ///     Tries to change the theme to a specific theme, this can be either a default or a custom defined one.
    /// </summary>
    /// <param name="theme"></param>
    public void ChangeColorTheme(ColorTheme theme) =>
        SetColorTheme(theme);

    /// <summary>
    ///     Blindly switches to the "next" theme available in the <see cref="ColorThemes" /> collection.
    /// </summary>
    public void SwitchColorTheme()
    {
        var index = -1;
        for (var i = 0; i < ColorThemes.Count; i++)
        {
            if (ColorThemes[i] != ActiveColorTheme) continue;
            index = i;
            break;
        }
        if (index == -1) return;
        var newIndex = (index + 1) % ColorThemes.Count;
        var newColorTheme = ColorThemes[newIndex];
        ChangeColorTheme(newColorTheme);
    }

    /// <summary>
    ///     Add a new <see cref="ColorTheme" /> to the ones available, without making it active.
    /// </summary>
    /// <param name="theme">New <see cref="ColorTheme" /> to add.</param>
    public void AddColorTheme(ColorTheme theme)
    {
        if (!_colorThemeHashset.Add(theme))
            throw new InvalidOperationException("This color theme has already been added.");
        _allThemes.Add(theme);
    }

    /// <summary>
    ///     Adds multiple new <see cref="ColorTheme" /> to the ones available, without making any active.
    /// </summary>
    /// <param name="themes">A collection of new <see cref="ColorTheme" /> to add.</param>
    public void AddColorThemes(IEnumerable<ColorTheme> themes)
    {
        foreach (var colorTheme in themes)
            AddColorTheme(colorTheme);
    }

    /// <summary>
    ///     Tries to change the base theme to the one provided, if it is different.
    /// </summary>
    /// <param name="baseTheme"><see cref="ThemeVariant" /> to change to.</param>
    public void ChangeBaseTheme(ThemeVariant baseTheme)
    {
        if (_app.ActualThemeVariant == baseTheme) return;
        _app.RequestedThemeVariant = baseTheme;
    }

    /// <summary>
    ///     Simply switches from Light -> Dark and visa-versa.
    /// </summary>
    public void SwitchBaseTheme()
    {
        if (Application.Current is null) return;
        var newBase = Application.Current.ActualThemeVariant == ThemeVariant.Dark
            ? ThemeVariant.Light
            : ThemeVariant.Dark;
        Application.Current.RequestedThemeVariant = newBase;
    }

    private void UpdateFlowDirectionResources(bool rightToLeft)
    {
        var primary = rightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        var opposite = rightToLeft ? FlowDirection.LeftToRight : FlowDirection.RightToLeft;

        Resources["FlowDirectionPrimary"] = primary;
        Resources["FlowDirectionOpposite"] = opposite;
    }

    /// <summary>
    ///     Initializes the color theme resources whenever the property is changed.
    ///     In an ideal world people wouldn't use the property
    /// </summary>
    private void SetColorThemeResourcesOnColorThemeChanged()
    {
        if (!DefaultColorThemes.TryGetValue(ThemeColor, out var colorTheme))
            throw new Exception($"{ThemeColor} has no defined color theme.");
        SetColorTheme(colorTheme);
    }

    private void SetColorTheme(ColorTheme colorTheme)
    {
        SetColorWithOpacities("PrimaryColor", colorTheme.Primary);
        SetResource("PrimaryDarkColor", colorTheme.PrimaryDark);
        SetColorWithOpacities("AccentColor", colorTheme.Accent);
        SetResource("AccentDarkColor", colorTheme.AccentDark);
        ActiveColorTheme = colorTheme;
        OnColorThemeChanged?.Invoke(ActiveColorTheme);
    }

    private void SetColorWithOpacities(string baseName, Color baseColor)
    {
        SetResource(baseName, baseColor);
        SetResource($"{baseName}75", baseColor.WithAlpha(0.75));
        SetResource($"{baseName}50", baseColor.WithAlpha(0.50));
        SetResource($"{baseName}25", baseColor.WithAlpha(0.25));
        SetResource($"{baseName}20", baseColor.WithAlpha(0.2));
        SetResource($"{baseName}15", baseColor.WithAlpha(0.15));
        SetResource($"{baseName}10", baseColor.WithAlpha(0.10));
        SetResource($"{baseName}7", baseColor.WithAlpha(0.07));
        SetResource($"{baseName}5", baseColor.WithAlpha(0.05));
        SetResource($"{baseName}3", baseColor.WithAlpha(0.03));
        SetResource($"{baseName}1", baseColor.WithAlpha(0.005));
        SetResource($"{baseName}0", baseColor.WithAlpha(0.00));
    }

    private void SetResource(string name, Color color) =>
        _app.Resources[name] = color;

    /// <summary>
    ///     The default Color Themes included with UI.
    /// </summary>
    public static readonly IReadOnlyDictionary<BaseColor, ColorTheme> DefaultColorThemes;

    static BaseTheme()
    {
        var defaultThemes = new[]
        {
            new DefaultColorTheme(BaseColor.Orange, Color.Parse("#d48806"), Color.Parse("#176CE8")),
            new DefaultColorTheme(BaseColor.Red, Color.Parse("#D03A2F"), Color.Parse("#2FC5D0")),
            new DefaultColorTheme(BaseColor.Green, Color.Parse("#537834"), Color.Parse("#B24DB0")),
            new DefaultColorTheme(BaseColor.Blue, Color.Parse("#0A59F7"), Color.Parse("#F7A80A"))
        };
        DefaultColorThemes = defaultThemes.ToDictionary(x => x.ThemeColor, y => (ColorTheme) y);
    }

    /// <summary>
    ///     Retrieves an instance tied to a specific instance of an application.
    /// </summary>
    /// <returns>A <see cref="BaseTheme" /> instance that can be used to change themes.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no Theme has been defined in App.axaml.</exception>
    public static BaseTheme GetInstance(Application app)
    {
        var theme = app.Styles.FirstOrDefault(style => style is BaseTheme);
        if (theme is not BaseTheme baseTheme)
            throw new InvalidOperationException(
                "No Theme instance available. Ensure theme has been set in Application.Styles in App.axaml.");
        return baseTheme;
    }

    /// <summary>
    ///     Retrieves an instance tied to the currently active application.
    /// </summary>
    /// <returns>A <see cref="BaseTheme" /> instance that can be used to change themes.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no Theme has been defined in App.axaml.</exception>
    public static BaseTheme GetInstance() => GetInstance(Application.Current!);
}