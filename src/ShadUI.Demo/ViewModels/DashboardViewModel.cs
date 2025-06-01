using System;
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

    private const string FontName = "Geist";

    [ObservableProperty]
    private static SolidColorPaint _tooltipTextPaint = new()
    {
        Color = SKColors.Black,
        SKTypeface = SKTypeface.FromFamilyName(FontName)
    };

    public DashboardViewModel(ThemeWatcher themeWatcher)
    {
        ThemeWatcher = themeWatcher;
        ThemeWatcher.ThemeChanged += (_, colors) =>
        {
            UpdateAxesLabelPaints(colors);
            UpdateSeriesFill(colors.PrimaryColor);
        };
    }

    public void Initialize()
    {
        ((ColumnSeries<double>)Series[0]).Values = GenerateRandomValues();
        var primary = ThemeWatcher.ThemeColors.PrimaryColor;
        UpdateSeriesFill(primary);
    }

    private void UpdateSeriesFill(Color primary)
    {
        var color = new SKColor(primary.R, primary.G, primary.B, primary.A);
        if (Series.Length > 0) ((ColumnSeries<double>)Series[0]).Fill = new SolidColorPaint(color);
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
            SKTypeface = SKTypeface.FromFamilyName(FontName)
        };

        XAxes[0].LabelsPaint = foregroundPaint;
        YAxes[0].LabelsPaint = foregroundPaint;
    }

    public ISeries[] Series { get; set; } =
    [
        new ColumnSeries<double>
        {
            Values = GenerateRandomValues(),
            Fill = new SolidColorPaint(SKColors.Transparent)
        }
    ];

    private static double[] GenerateRandomValues()
    {
        var random = new Random();

        var values = new double[12];
        for (var i = 0; i < values.Length; i++)
        {
            values[i] = random.Next(1000, 5000);
        }

        return values;
    }

    public Axis[] XAxes { get; set; } =
    [
        new()
        {
            Labels = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            LabelsPaint = new SolidColorPaint
            {
                Color = SKColors.Gray,
                SKTypeface = SKTypeface.FromFamilyName(FontName)
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
                SKTypeface = SKTypeface.FromFamilyName(FontName)
            },
            TextSize = 12,
            MinStep = 1500,
            ShowSeparatorLines = false
        }
    ];
}