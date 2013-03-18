using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Sparrow.Chart
{
    /// <summary>
    /// SplineSeries Container
    /// </summary>
    public class SplineContainer : SeriesContainer
    {
        public override void Draw()
        {
            base.Draw();
            
        }
        protected override void DrawPath(SeriesBase series, System.Drawing.Pen pen)
        {
            if (series is SplineSeries)
            {
                SplineSeries splineSeries = series as SplineSeries;
                var points = splineSeries.SplinePoints;
                var pointCount = splineSeries.SplinePoints.Count;
                if (RenderingMode == RenderingMode.DefaultWPFRendering)
                {
                    PartsCanvas.Children.Clear();
                    for (int i = 0; i < splineSeries.Parts.Count; i++)
                    {
                        PartsCanvas.Children.Add(splineSeries.Parts[i].CreatePart());
                    }
                }
                else
                {
                    for (int i = 0; i < pointCount - 1; i++)
                    {
                        switch (RenderingMode)
                        {
                            case RenderingMode.GDIRendering:
                                GDIGraphics.DrawBezier(pen, points[i].AsDrawingPointF(), splineSeries.FirstControlPoints[i].AsDrawingPointF(), splineSeries.SecondControlPoints[i].AsDrawingPointF(), points[i + 1].AsDrawingPointF());
                                break;
                            case RenderingMode.DefaultWPFRendering:
                                break;
                            case RenderingMode.WritableBitmap:
                                this.WritableBitmap.Lock();
                                WritableBitmapGraphics.DrawBezier(pen, points[i].AsDrawingPointF(), splineSeries.FirstControlPoints[i].AsDrawingPointF(), splineSeries.SecondControlPoints[i].AsDrawingPointF(), points[i + 1].AsDrawingPointF());
                                this.WritableBitmap.AddDirtyRect(new Int32Rect(0, 0, WritableBitmap.PixelWidth, WritableBitmap.PixelHeight));
                                this.WritableBitmap.Unlock();
                                break;
                            default:
                                break;
                        }
                    }
                    this.collection.InvalidateBitmap();
                }
            }
        }
    }
}
