using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;
using Image=System.Windows.Controls.Image;
using System.Windows.Threading;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
#endif
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
#if WPF
        protected override void DrawPath(SeriesBase series, System.Drawing.Pen pen)
        {
            if (series is SplineSeries)
            {
                SplineSeries splineSeries = series as SplineSeries;
                var points = splineSeries.SplinePoints;
                var pointCount = splineSeries.SplinePoints.Count;
                if (RenderingMode == RenderingMode.Default)
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
                            case RenderingMode.Default:
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
#else
        protected override void DrawPath(SeriesBase series, Brush brush, double strokeThickness)
        {
            if (series is SplineSeries)
            {
                SplineSeries splineSeries = series as SplineSeries;
                PartsCanvas.Children.Clear();
                for (int i = 0; i < splineSeries.Parts.Count; i++)
                {
                    PartsCanvas.Children.Add(splineSeries.Parts[i].CreatePart());
                }
            }
        }
#endif
    }
}
