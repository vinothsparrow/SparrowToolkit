using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Sparrow.Chart
{
    /// <summary>
    /// StepLineSeries Container
    /// </summary>
    public class StepLineContainer : SeriesContainer
    {
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
                var pointCount = ((StepLineSeries)Series).LinePoints.Count;
                var points = ((StepLineSeries)Series).LinePoints;
                for (int i = 0; i < pointCount - 1; i++)
                {
                    directXBrush = collection.GetDirectXBrush((this.Series as StepLineSeries).Stroke);
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
        override protected void DrawPath(SeriesBase series, System.Drawing.Pen pen)
        {
            if (series is StepLineSeries)
            {
                StepLineSeries stepLineSeries = series as StepLineSeries;
                var points = stepLineSeries.LinePoints;
                var pointCount = stepLineSeries.LinePoints.Count;
                if (RenderingMode == RenderingMode.DefaultWPFRendering)
                {
                    PartsCanvas.Children.Clear();
                    for (int i = 0; i < stepLineSeries.Parts.Count; i++)
                    {
                        PartsCanvas.Children.Add(stepLineSeries.Parts[i].CreatePart());
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
                                WritableBitmapGraphics.DrawLine(pen, points[i].AsDrawingPointF(), points[i + 1].AsDrawingPointF());
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
