using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Sparrow.Chart
{
    /// <summary>
    /// Area Container for AreaSeries
    /// </summary>
    public class AreaContainer : SeriesContainer
    {
        public override void Draw()
        {
            base.Draw();        
        }
        private void DrawFilledPath(SeriesBase series, System.Drawing.Pen pen,System.Drawing.Brush brush)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            
            if (series is AreaSeries)
            {
                path.StartFigure();
                AreaSeries areaSeries = series as AreaSeries;
                var points = areaSeries.AreaPoints;
                var pointCount = areaSeries.AreaPoints.Count;                
                for (int i = 0; i < pointCount - 1; i++)
                {
                    System.Drawing.PointF startPoint = points[i].AsDrawingPointF();
                    System.Drawing.PointF endPoint = points[i + 1].AsDrawingPointF();                    
                    path.AddLine(startPoint, endPoint);                    
                }       
                
                path.CloseAllFigures();

                switch (RenderingMode)
                {
                    case RenderingMode.GDIRendering:
                        GDIGraphics.FillPath(brush, path);
                        break;
                    case RenderingMode.DefaultWPFRendering:
                        break;
                    case RenderingMode.WritableBitmap:
                        WritableBitmapGraphics.FillPath(brush, path);
                        break;
                    default:
                        break;
                }                
            }
           
        }
        protected override void DrawPath(SeriesBase series, System.Drawing.Pen pen)
        {
            if (series is AreaSeries)
            {
                AreaSeries areaSeries = series as AreaSeries;
                var brush = (this.Series as AreaSeries).Fill.AsDrawingBrush();
                var points = areaSeries.AreaPoints;
                var pointCount = areaSeries.AreaPoints.Count;
                if (RenderingMode == RenderingMode.DefaultWPFRendering)
                {
                    PartsCanvas.Children.Clear();
                    for (int i = 0; i < areaSeries.Parts.Count; i++)
                    {
                        PartsCanvas.Children.Add(areaSeries.Parts[i].CreatePart());
                    }
                }
                else
                {
                    for (int i = 0; i < pointCount - 1; i++)
                    {
                        switch (RenderingMode)
                        {
                            case RenderingMode.GDIRendering:
                                GDIGraphics.DrawLine(pen, points[i].AsDrawingPointF(), points[i + 1].AsDrawingPointF());
                                break;
                            case RenderingMode.DefaultWPFRendering:
                                break;
                            case RenderingMode.WritableBitmap:
                                this.WritableBitmap.Lock();
                                WritableBitmapGraphics.DrawLine(pen, points[i].AsDrawingPointF(), points[i + 1].AsDrawingPointF());
                                this.WritableBitmap.AddDirtyRect(new Int32Rect(0, 0, WritableBitmap.PixelWidth, WritableBitmap.PixelHeight));
                                this.WritableBitmap.Unlock();
                                break;
                            default:
                                break;
                        }
                    }
                    DrawFilledPath(areaSeries, pen, brush);
                }
                this.collection.InvalidateBitmap();
            }
        }
    }
}
