using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ShadUI.Themes;
using SkiaSharp;

namespace ShadUI.Demo.ViewModels;

public sealed partial class DashboardViewModel : ViewModelBase
{
    public ThemeWatcher ThemeWatcher { get; }

    [ObservableProperty]
    private static SolidColorPaint _tooltipTextPaint = new()
    {
        Color = SKColors.Black,
        SKTypeface = SKTypeface.FromFamilyName("Inter")
    };

    public DashboardViewModel(ThemeWatcher themeWatcher)
    {
        ThemeWatcher = themeWatcher;
        ThemeWatcher.ThemeChanged += (_, colors) => UpdateThemeColors(colors);
    }

    public void Initialize()
    {
        UpdateThemeColors(ThemeWatcher.ThemeColors);
    }

    private void UpdateThemeColors(ThemeColors colors)
    {
        UpdateAxesLabelPaints(colors);
        UpdateSeriesFill(colors.PrimaryColor);
    }

    private void UpdateAxesLabelPaints(ThemeColors colors)
    {
        var foreground = new SKColor(
            colors.ForegroundColor.R,
            colors.ForegroundColor.G,
            colors.ForegroundColor.B,
            colors.ForegroundColor.A);
        var foregroundPaint = new SolidColorPaint
        {
            Color = foreground,
            SKTypeface = SKTypeface.FromFamilyName("Inter")
        };

        XAxes[0].LabelsPaint = foregroundPaint;
        YAxes[0].LabelsPaint = foregroundPaint;
    }

    private void UpdateSeriesFill(Color color)
    {
        var skColor = new SKColor(color.R, color.G, color.B, color.A);
        if (Series.Length > 0) ((ColumnSeries<double>) Series[0]).Fill = new SolidColorPaint(skColor);
    }

    public ISeries[] Series { get; set; } =
    [
        new ColumnSeries<double>
        {
            Values = [1300, 2700, 2950, 1750, 1259, 4490, 5500, 1550, 1540, 4450, 3000, 3200],
            Fill = new SolidColorPaint(SKColors.Transparent)
        }
    ];

    public Axis[] XAxes { get; set; } =
    [
        new()
        {
            Labels = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            LabelsPaint = new SolidColorPaint
            {
                Color = SKColors.Gray,
                SKTypeface = SKTypeface.FromFamilyName("Inter")
            },
            TextSize = 12,
            MinStep = 1
        }
    ];

    public Axis[] YAxes { get; set; } =
    [
        new()
        {
            Labeler = Labelers.Currency,
            LabelsPaint = new SolidColorPaint
            {
                Color = SKColors.Gray,
                SKTypeface = SKTypeface.FromFamilyName("Inter")
            },
            TextSize = 12,
            MinStep = 1500,
            ShowSeparatorLines = false
        }
    ];
}