using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
#if WPF
using System.Drawing;
using System.Drawing.Drawing2D;
#endif
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
#if WPF
        internal Graphics GDIGraphics;
        internal InteropBitmap InteropBitmap;
        internal WriteableBitmap WritableBitmap;
        internal Bitmap ImageBitmap;
        internal Graphics WritableBitmapGraphics;
        internal double dpiFactor;
#endif
        internal Canvas PartsCanvas;
        internal ContainerCollection collection;
#if WINRT
        IAsyncAction action;
#endif

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
            DependencyProperty.Register("RenderingMode", typeof(RenderingMode), typeof(SeriesContainer), new PropertyMetadata(RenderingMode.Default));

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
#if WPF
                    case RenderingMode.GDIRendering:
                        if (GDIGraphics != null)
                            GDIGraphics.Clear(System.Windows.Media.Colors.Transparent.AsDrawingColor());
                        break;
                     case RenderingMode.WritableBitmap:
                        if (WritableBitmapGraphics != null)
                            WritableBitmapGraphics.Clear(System.Windows.Media.Colors.Transparent.AsDrawingColor());
                        break;
#endif
                    case RenderingMode.Default:
                        PartsCanvas.Children.Clear();
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
#if WPF
                case RenderingMode.GDIRendering:
                    if (this.InteropBitmap != null && this.GDIGraphics != null)
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
#endif
#if DIRECTX2D
                case RenderingMode.DirectX2D:
#endif
                case RenderingMode.Default:
#if WPF
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        Draw();
                    }));
                    break;
#elif !WINRT 
                    this.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        Draw();
                    }));
                    break;
#endif
#if WINRT
                    action=this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,delegate
                    {
                        Draw();
                    });
                    break;
#endif
                default:
                    break;
            }           

        }
        public virtual void Draw()
        {
            if (this.Series.Index == 0)
                Clear();
#if WPF
            var brush = this.Series.Stroke.AsDrawingBrush();
            var pen = new System.Drawing.Pen(brush, (float)this.Series.StrokeThickness);
#endif
#if DIRECTX2D
            thickness = (float)(this.Series as LineSeries).StrokeThickness;
#endif

            switch (this.RenderingMode)
            {
#if WPF
                case RenderingMode.GDIRendering:
                    if (Series != null)
                        DrawPath(Series, pen);
                    break;
                 case RenderingMode.WritableBitmap:
                    if (Series != null)
                        DrawPath(Series, pen);
                    break;
#endif
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
                case RenderingMode.Default:
                    Clear();
                    if (Series != null)
#if WPF
                        DrawPath(Series, pen);
#else
                        DrawPath(Series, this.Series.Stroke,this.Series.StrokeThickness);
#endif
                    break;
                
                default:
                    break;
            }
        }

#if WPF
        protected virtual void DrawPath(SeriesBase series, System.Drawing.Pen pen)
#else
        protected virtual void DrawPath(SeriesBase series, Brush brush,double strokeThickness)
#endif
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
