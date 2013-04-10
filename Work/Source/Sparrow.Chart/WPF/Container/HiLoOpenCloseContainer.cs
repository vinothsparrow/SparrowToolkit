using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;
using Image = System.Windows.Controls.Image;
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

#if WPF
using System.Drawing;
using System.Drawing.Drawing2D;
using LinearGradientBrush = System.Drawing.Drawing2D.LinearGradientBrush;
#endif

namespace Sparrow.Chart
{
    public class HiLoOpenCloseContainer : SeriesContainer
    {
        public HiLoOpenCloseContainer()
            : base()
        {

        }
        public override void Draw()
        {
            base.Draw();
        }
        #if DIRECTX2D
        override protected void OnRender()
        {
            if (this.Directx2DGraphics.IsFrontBufferAvailable)
            {
                this.Directx2DGraphics.Lock();
                this.Directx2DGraphics.AddDirtyRect(new Int32Rect(0, 0, this.Directx2DGraphics.PixelWidth, this.Directx2DGraphics.PixelHeight));
                this.Directx2DGraphics.Unlock();
            }

            if (this.RenderTarget != null)
            {
                this.RenderTarget.BeginDraw();
                if (this.Series.Index == 0)
                    this.RenderTarget.Clear(new ColorF(0, 0, 0, 0.0f));
                var pointCount = ((LineSeries)Series).LinePoints.Count;
                var points = ((LineSeries)Series).LinePoints;
                for (int i = 0; i < pointCount - 1; i++)
                {
                    directXBrush = collection.GetDirectXBrush((this.Series as LineSeries).Stroke);
                    this.RenderTarget.DrawLine(points[i].AsDirectX2DPoint(), points[i + 1].AsDirectX2DPoint(), directXBrush, thickness);
                }
                this.RenderTarget.EndDraw();
            }
        }
        
        protected void OnFreeResources()
        {
            collection.OnFreeResources();
            if (this.directXBrush != null)
            {
                this.directXBrush.Dispose();
                this.directXBrush = null;
            }            
        }

       
#endif
#if WPF
        override protected void DrawPath(SeriesBase series, System.Drawing.Pen pen)
        {
            if (series is HiLoOpenCloseSeries)
            {
                var points = new PointCollection();
                var lowPoints = new PointCollection();
                var openPoints = new PointCollection();
                var closePoints = new PointCollection();
                var openOffPoints = new PointCollection();
                var closeOffPoints = new PointCollection();
                var pointCount = 0;
                PartsCollection partsCollection = new PartsCollection();
                if (series is HiLoOpenCloseSeries)
                {
                    HiLoOpenCloseSeries lineSeries = series as HiLoOpenCloseSeries;
                    points = lineSeries.HighPoints;
                    lowPoints = lineSeries.LowPoints;
                    openPoints = lineSeries.OpenPoints;
                    closePoints = lineSeries.ClosePoints;
                    openOffPoints = lineSeries.openOffPoints;
                    closeOffPoints = lineSeries.closeOffPoints;

                    pointCount = lineSeries.HighPoints.Count;
                    partsCollection = lineSeries.Parts;
                }
                if (RenderingMode == RenderingMode.Default)
                {
                    for (int i = 0; i < partsCollection.Count; i++)
                    {
                        PartsCanvas.Children.Add(partsCollection[i].CreatePart());
                    }
                }
                else
                {
                    if (series is HiLoOpenCloseSeries)
                    {
                        for (int i = 0; i < pointCount; i++)
                        {

                            switch (RenderingMode)
                            {
                                case RenderingMode.GDIRendering:
                                    GDIGraphics.DrawLine(pen, points[i].AsDrawingPointF(), lowPoints[i].AsDrawingPointF());
                                    GDIGraphics.DrawLine(pen, openOffPoints[i].AsDrawingPointF(), openPoints[i].AsDrawingPointF());
                                    GDIGraphics.DrawLine(pen, closeOffPoints[i].AsDrawingPointF(), closePoints[i].AsDrawingPointF());
                                    break;
                                case RenderingMode.Default:
                                    break;
                                case RenderingMode.WritableBitmap:
                                    this.WritableBitmap.Lock();
                                    WritableBitmapGraphics.DrawLine(pen, points[i].AsDrawingPointF(), lowPoints[i].AsDrawingPointF());
                                    WritableBitmapGraphics.DrawLine(pen, openOffPoints[i].AsDrawingPointF(), openPoints[i].AsDrawingPointF());
                                    WritableBitmapGraphics.DrawLine(pen, closeOffPoints[i].AsDrawingPointF(), closePoints[i].AsDrawingPointF());
                                    this.WritableBitmap.Unlock();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }                   
                    this.collection.InvalidateBitmap();
                }                
            }        
        }
#else
        protected override void DrawPath(SeriesBase series, Brush brush, double strokeThickness)
        {
            if (series is HiLoOpenCloseSeries)
            {
                var points = new PointCollection();
                var pointCount = 0;
                PartsCollection partsCollection = new PartsCollection();
                partsCollection = (series as HiLoOpenCloseSeries).Parts;
                if (RenderingMode == RenderingMode.Default)
                {
                    for (int i = 0; i < partsCollection.Count; i++)
                    {
                        PartsCanvas.Children.Add(partsCollection[i].CreatePart());
                    }
                }
            }
        }
#endif
    }
}
