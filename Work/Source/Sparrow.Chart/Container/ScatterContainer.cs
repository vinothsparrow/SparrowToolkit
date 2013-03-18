using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Sparrow.Chart
{
    /// <summary>
    /// Scatter Series Container
    /// </summary>
    public class ScatterContainer : SeriesContainer
    {
        public override void Draw()
        {
            base.Draw();
        }
       
        override protected void DrawPath(SeriesBase series, System.Drawing.Pen pen)
        {
            if (series is ScatterSeries)
            {
                ScatterSeries scatterSeries = series as ScatterSeries;
                var points = scatterSeries.ScatterPoints;
                var pointCount = scatterSeries.ScatterPoints.Count;
                float size = (float)scatterSeries.ScatterSize;
                var brush = (this.Series as ScatterSeries).Fill.AsDrawingBrush();
                if (RenderingMode == RenderingMode.DefaultWPFRendering)
                {
                    for (int i = 0; i < scatterSeries.Parts.Count; i++)
                    {
                        Ellipse element=(Ellipse)scatterSeries.Parts[i].CreatePart();
                        Canvas.SetLeft(element, points[i].X - (element.Width / 2));
                        Canvas.SetTop(element, points[i].Y - (element.Height / 2));
                        PartsCanvas.Children.Add(element);                        
                    }
                }
                else
                {
                    for (int i = 0; i < pointCount; i++)
                    {
                        switch (RenderingMode)
                        {
                            case RenderingMode.GDIRendering:
                                GDIGraphics.DrawEllipse(pen, points[i].AsDrawingPointF().X - size / 2, points[i].AsDrawingPointF().Y - size / 2, size, size);
                                GDIGraphics.FillEllipse(brush, points[i].AsDrawingPointF().X - size / 2, points[i].AsDrawingPointF().Y - size / 2, size, size);
                                break;
                            case RenderingMode.DefaultWPFRendering:
                                break;
                            case RenderingMode.WritableBitmap:
                                this.WritableBitmap.Lock();
                                WritableBitmapGraphics.DrawEllipse(pen, points[i].AsDrawingPointF().X - size / 2, points[i].AsDrawingPointF().Y - size / 2, size, size);
                                WritableBitmapGraphics.FillEllipse(brush, points[i].AsDrawingPointF().X - size / 2, points[i].AsDrawingPointF().Y - size / 2, size, size);
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
