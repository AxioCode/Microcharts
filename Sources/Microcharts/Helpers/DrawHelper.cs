using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microcharts
{
    internal static class DrawHelper
    {
        internal static void DrawLabel(SKCanvas canvas, Orientation orientation, bool isTop, SKSize itemSize, SKPoint point, SKColor color, SKRect bounds, string text, float textSize, SKTypeface typeface)
        {
            using (new SKAutoCanvasRestore(canvas))
            {
                using (var paint = new SKPaint())
                {
                    paint.TextSize = textSize;
                    paint.IsAntialias = true;
                    paint.Color = color;
                    paint.IsStroke = false;
                    paint.Typeface = typeface;

                    if (orientation == Orientation.Vertical)
                    {
                        var y = point.Y;

                        if (isTop)
                        {
                            y -= bounds.Width;
                        }

                        canvas.RotateDegrees(90);
                        canvas.Translate(y, -point.X + (bounds.Height / 2));
                    }
                    else
                    {
                        if (bounds.Width > itemSize.Width)
                        {
                            text = text.Substring(0, Math.Min(3, text.Length));
                            paint.MeasureText(text, ref bounds);
                        }

                        if (bounds.Width > itemSize.Width)
                        {
                            text = text.Substring(0, Math.Min(1, text.Length));
                            paint.MeasureText(text, ref bounds);
                        }

                        var y = point.Y;

                        if (isTop)
                        {
                            y -= bounds.Height;
                        }

                        canvas.Translate(point.X - (bounds.Width / 2), y);
                    }

                    canvas.DrawText(text, 0, 0, paint);
                }
            }
        }

        internal static void DrawYAxis(bool showYAxisText, bool showYAxisLines, Position yAxisPosition, SKPaint yAxisTextPaint, SKPaint yAxisLinesPaint, float margin, float animationProgress, float maxValue, float valueRange, SKCanvas canvas, int width, float yAxisXShift, List<float> yAxisIntervalLabels, float headerHeight, SKSize itemSize, float origin)
        {
            if (showYAxisText || showYAxisLines)
            {
                int cnt = 0;
                var intervals = yAxisIntervalLabels
                    .Select(t => new ValueTuple<string, SKPoint>
                    (
                        t.ToString(),
                        new SKPoint(yAxisPosition == Position.Left ? yAxisXShift : width, MeasureHelper.CalculatePoint(margin, animationProgress, maxValue, valueRange, t, cnt++, itemSize, origin, headerHeight).Y)
                    ))
                    .ToList();

                if (showYAxisText)
                {
                    DrawYAxisText(yAxisTextPaint, yAxisPosition, canvas, intervals);
                }

                if (showYAxisLines)
                {
                    var lines = intervals.Select(tup =>
                    {
                        (_, SKPoint pt) = tup;

                        return yAxisPosition == Position.Right ?
                            SKRect.Create(0, pt.Y, width, 0) :
                            SKRect.Create(yAxisXShift, pt.Y, width, 0);
                    });

                    DrawYAxisLines(margin, yAxisLinesPaint, canvas, lines);
                }
            }
        }

        internal static void DrawXAxis(bool showXAxisLines, SKPaint xAxisLinesPaint, float margin, float animationProgress, float maxValue, float valueRange, SKCanvas canvas, int height, List<float> xAxisIntervalLabels, float footerHeight, SKSize itemSize, float origin)
        {
            if (showXAxisLines)
            {
                int cnt = 0;
                var intervals = xAxisIntervalLabels
                    .Select(value => new ValueTuple<string, SKPoint>
                    (
                        value.ToString(),
                        MeasureHelper.CalculatePoint(margin, animationProgress, maxValue, valueRange, value, cnt++, itemSize, origin, footerHeight)
                    ))
                    .ToList();

                if (showXAxisLines)
                {
                    var axisMarginWithLabels = 20;
                    var lines = intervals.Select(tup =>
                    {
                        (_, SKPoint pt) = tup;
                        return SKRect.Create(pt.X, (footerHeight + axisMarginWithLabels), 0, (origin - footerHeight - axisMarginWithLabels));
                    });

                    DrawXAxisLines(margin, xAxisLinesPaint, canvas, lines);
                }
            }
        }

        /// <summary>
        /// Shows a Y axis
        /// </summary>
        /// <param name="yAxisTextPaint"></param>
        /// <param name="yAxisPosition"></param>
        /// <param name="canvas"></param>
        /// <param name="intervals"></param>
        private static void DrawYAxisText(SKPaint yAxisTextPaint, Position yAxisPosition, SKCanvas canvas, IEnumerable<(string Label, SKPoint Point)> intervals)
        {
            var pt = yAxisTextPaint.Clone();
            pt.TextAlign = yAxisPosition == Position.Left ? SKTextAlign.Right : SKTextAlign.Left;

            foreach (var @int in intervals)
                canvas.DrawTextCenteredVertically(@int.Label, pt, @int.Point.X, @int.Point.Y);
        }

        /// <summary>
        /// Draws Y axis lines
        /// </summary>
        /// <param name="Margin">Graph margin</param>
        /// <param name="yAxisLinesPaint">SKPaint apply to lines</param>
        /// <param name="canvas">Graph canvas</param>
        /// <param name="intervals">Lines are prepared a step before in intervals</param>
        private static void DrawYAxisLines(float Margin, SKPaint yAxisLinesPaint, SKCanvas canvas, IEnumerable<SKRect> intervals)
        {
            foreach (var @int in intervals)
            {
                canvas.DrawLine(Margin / 2 + @int.Left, @int.Top, @int.Right - Margin / 2, @int.Bottom, yAxisLinesPaint);
            }
        }

        /// <summary>
        /// Draws X axis lines
        /// </summary>
        /// <param name="Margin">Graph margin</param>
        /// <param name="xAxisLinesPaint">SKPaint apply to lines</param>
        /// <param name="canvas">Graph canvas</param>
        /// <param name="intervals">Lines are prepared a step before in intervals</param>
        private static void DrawXAxisLines(float Margin, SKPaint xAxisLinesPaint, SKCanvas canvas, IEnumerable<SKRect> intervals)
        {
            foreach (var @int in intervals)
            {
                canvas.DrawLine(@int.Left, @int.Top, @int.Right, @int.Bottom, xAxisLinesPaint);
            }
        }
    }
}
