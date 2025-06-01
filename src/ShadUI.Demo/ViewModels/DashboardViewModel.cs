using System;
using Avalonia.Media;
using Avalonia.Platform;
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

    private readonly SKTypeface _typeface;

    [ObservableProperty]
    private static SolidColorPaint _tooltipTextPaint = null!;

    public DashboardViewModel(ThemeWatcher themeWatcher)
    {
        ThemeWatcher = themeWatcher;
        ThemeWatcher.ThemeChanged += (_, colors) =>
        {
            UpdateAxesLabelPaints(colors);
            UpdateSeriesFill(colors.PrimaryColor);
        };

        var fontUri = new Uri("avares://ShadUI.Demo/Assets/Fonts/Geist-Regular.ttf");
        var fontAsset = AssetLoader.Open(fontUri);

        using var skData = SKData.Create(fontAsset);
        _typeface = SKTypeface.FromData(skData);

        _tooltipTextPaint = new SolidColorPaint
        {
            Color = SKColors.Black,
            SKTypeface = _typeface
        };

        XAxes =
        [
            new Axis
            {
                Labels = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                LabelsPaint = new SolidColorPaint { Color = SKColors.Gray, SKTypeface = _typeface },
                TextSize = 12,
                MinStep = 1
            }
        ];

        YAxes =
        [
            new Axis
            {
                Labeler = Labelers.Currency,
                LabelsPaint = new SolidColorPaint { Color = SKColors.Gray, SKTypeface = _typeface },
                TextSize = 12,
                MinStep = 1500,
                ShowSeparatorLines = false
            }
        ];
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
            SKTypeface = _typeface
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

    public Axis[] XAxes { get; set; }

    public Axis[] YAxes { get; set; }
}