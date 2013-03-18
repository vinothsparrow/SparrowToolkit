using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;
using Image=System.Windows.Controls.Image;
using System.Windows.Threading;
#if DIRECTX2D
using Sparrow.Directx2D;
using D2D = Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using D3D10 = Microsoft.WindowsAPICodePack.DirectX.Direct3D10;
using DirectX = Microsoft.WindowsAPICodePack.DirectX;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
#endif
namespace Sparrow.Chart
{
    /// <summary>
    /// SeriesBase Container
    /// </summary>
    public abstract class SeriesContainer : DependencyObject
    {
        internal Image sourceImage;
#if DIRECTX2D
        internal Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush directXBrush;
        internal float thickness;
        internal D2D.RenderTarget RenderTarget;
        internal D3D10Image Directx2DGraphics;
#endif
        internal Graphics GDIGraphics;
        internal InteropBitmap InteropBitmap;
        internal WriteableBitmap WritableBitmap;
        internal Bitmap ImageBitmap;
        internal Graphics WritableBitmapGraphics;
        internal double dpiFactor;
        internal Canvas PartsCanvas;
       

        internal ContainerCollection collection;

        public SeriesBase Series
        {
            get { return (SeriesBase)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series", typeof(SeriesBase), typeof(SeriesContainer), new PropertyMetadata(null));

        public RenderingMode RenderingMode
        {
            get { return (RenderingMode)GetValue(RenderingModeProperty); }
            set { SetValue(RenderingModeProperty, value); }
        }

        public static readonly DependencyProperty RenderingModeProperty =
            DependencyProperty.Register("RenderingMode", typeof(RenderingMode), typeof(SeriesContainer), new PropertyMetadata(RenderingMode.WritableBitmap));

        public SeriesContainer()
        {
            
            this.sourceImage = new Image() { Stretch = Stretch.None };
            PartsCanvas = new Canvas();
                      
        }
        
        public void Clear()
        {
            try
            {
                switch (RenderingMode)
                {
                    case RenderingMode.GDIRendering:
                        if (GDIGraphics != null)
                            GDIGraphics.Clear(System.Windows.Media.Colors.Transparent.AsDrawingColor());
                        break;
                    case RenderingMode.DefaultWPFRendering:
                        PartsCanvas.Children.Clear();
                        break;
                    case RenderingMode.WritableBitmap:
                        if (WritableBitmapGraphics != null)
                            WritableBitmapGraphics.Clear(System.Windows.Media.Colors.Transparent.AsDrawingColor());
                        break;
                    default:
                        break;
                }                
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception Occured : " + e.Message);
            }
        }
        private bool _isBitmapInitialized;

        public bool IsBitmapInitialized
        {
            get { return _isBitmapInitialized; }
            set { _isBitmapInitialized = value; }
        }

        public void Invalidate()
        {
            switch (RenderingMode)
            {
                case RenderingMode.GDIRendering:
                    if (this.InteropBitmap != null && this.GDIGraphics != null)
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            Draw();
                        }));
                    break;
#if DIRECTX2D
                case RenderingMode.DirectX2D:
#endif
                case RenderingMode.DefaultWPFRendering:
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        Draw();
                    }));
                    break;
                case RenderingMode.WritableBitmap:
                    if (this.WritableBitmap != null && this.WritableBitmapGraphics != null)
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            Draw();
                        }));
                    break;
                default:
                    break;
            }           

        }
        public virtual void Draw()
        {
            if (this.Series.Index == 0)
                Clear();
            var brush = this.Series.Stroke.AsDrawingBrush();
            var pen = new System.Drawing.Pen(brush, (float)this.Series.StrokeThickness);
#if DIRECTX2D
            thickness = (float)(this.Series as LineSeries).StrokeThickness;
#endif

            switch (this.RenderingMode)
            {
                case RenderingMode.GDIRendering:
                    if (Series != null)
                        DrawPath(Series, pen);
                    break;
#if DIRECTX2D
                case RenderingMode.DirectX2D:
                    this.collection.InvalidateBitmap();
                    this.Directx2DGraphics.Lock();
                    try
                    {         
                        this.OnRender();
                        collection.Render();
                    }
                    finally
                    {
                        this.Directx2DGraphics.Unlock();
                    }
                    break;
#endif
                case RenderingMode.DefaultWPFRendering:
                    Clear();
                    if (Series != null)
                        DrawPath(Series, pen);
                    break;
                case RenderingMode.WritableBitmap:
                    if (Series != null)
                        DrawPath(Series, pen);
                    break;
                default:
                    break;
            }
        }

        protected virtual void DrawPath(SeriesBase series, System.Drawing.Pen pen)
        {
        }
        protected virtual void OnRender()
        {
        }
             
        private int index;
        internal int Index
        {
            get { return index; }
            set { index = value; }
        }      
               
    }
}
