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
using Image = System.Windows.Controls.Image;
using System.Windows.Threading;
using Size = System.Windows.Size;
using System.Windows.Media.Animation;
using System.Windows.Data;
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

using System.IO;
#if DIRECTX2D
using Sparrow.Directx2D;
using D2D = Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using D3D10 = Microsoft.WindowsAPICodePack.DirectX.Direct3D10;
using DirectX = Microsoft.WindowsAPICodePack.DirectX;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
#endif
namespace Sparrow.Chart
{
    /// <summary>
    /// Container Panel
    /// </summary>
    public class ContainerCollection : Panel
    {
        public SeriesCollection Series
        {
            get { return (SeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series", typeof(SeriesCollection), typeof(ContainerCollection), new PropertyMetadata(null));

        Canvas containerCanvas;
#if DIRECTX2D
        protected Directx2DBitmap DirectxBimap;
        protected D3D10Image Directx2DGraphics;
        private readonly D2D.D2DFactory factory;
        private D3D10.D3DDevice device;
        private D2D.RenderTarget renderTarget;
        private D3D10.Texture2D texture;
       
#endif
#if WPF
        private bool disposed;
        protected bool isDirectXInitialized;
        private int bpp = PixelFormats.Bgra32.BitsPerPixel / 8;
        private IntPtr MapViewPointer;
        protected Graphics GDIGraphics;
        protected InteropBitmap InteropBitmap;
        internal double dpiFactor;
        protected WriteableBitmap WritableBitmap;
        protected Bitmap ImageBitmap;
        protected Graphics WritableBitmapGraphics;
#endif
        private Canvas partsCanvas;
        private bool isLegendUpdate;

        

        internal AxisCrossLinesContainer axisLinesconatiner;
        bool isIntialized;

        private const uint FILE_MAP_ALL_ACCESS = 0xF001F;
        private const uint PAGE_READWRITE = 0x04;

        

        public Containers Containers
        {
            get { return (Containers)GetValue(ContainersProperty); }
            set { SetValue(ContainersProperty, value); }
        }
        Image bitmapImage;
        public static readonly DependencyProperty ContainersProperty =
            DependencyProperty.Register("Containers", typeof(Containers), typeof(ContainerCollection), new PropertyMetadata(null));

        public ContainerCollection()
        {
            Containers = new Containers();
            this.SizeChanged += ContainerCollection_SizeChanged;
            bitmapImage = new Image();
            bitmapImage.Stretch = Stretch.None;
#if DIRECTX2D
             this.factory = D2D.D2DFactory.CreateFactory(D2D.D2DFactoryType.Multithreaded);
            this.Directx2DGraphics = new D3D10Image();
#endif            
            
        }

        void ContainerCollection_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if !WPF
            RectangleGeometry clipRectGeometry = new RectangleGeometry();
            clipRectGeometry.Rect = new Rect(new Point(0, 0), new Size(this.ActualWidth,this.ActualHeight));
            this.Clip = clipRectGeometry;
#endif
            
            Refresh();
        }
       
#if DIRECTX2D
        protected void InitializeDirectX()
        {
            this.Directx2DGraphics.Lock();
            try
            {
                this.CreateResources();

                // Resize to the size of this control, if we have a size

                width = Math.Max(1, (int)this.ActualWidth);
                height = Math.Max(1, (int)this.ActualHeight);
                this.Resize(width, height);

                this.Directx2DGraphics.SetBackBuffer(this.Texture);
                if (this.Directx2DGraphics.IsFrontBufferAvailable)
                {
                    this.Directx2DGraphics.Lock();
                    this.Directx2DGraphics.AddDirtyRect(new Int32Rect(0, 0, this.Directx2DGraphics.PixelWidth, this.Directx2DGraphics.PixelHeight));
                    this.Directx2DGraphics.Unlock();
                }
                 
            }
            finally
            {
                this.Directx2DGraphics.Unlock();
            }
            isDirectXInitialized = true;
        }
#endif
        public void Refresh()
        {
#if DIRECTX2D
            if (RenderingMode == RenderingMode.DirectX2D)
                InitializeDirectX();
#endif
            switch (RenderingMode)
            {
#if WPF
                case RenderingMode.GDIRendering:
                    if (this.InteropBitmap != null && this.GDIGraphics != null)
                    {
                        this.GDIGraphics.Dispose();
                        this.GDIGraphics = null;
                        this.InteropBitmap = null;
                        GC.Collect();
                    }
                    break;
               case RenderingMode.WritableBitmap:
                    if (this.WritableBitmap != null && this.WritableBitmapGraphics != null)
                    {
                        this.WritableBitmapGraphics.Dispose();
                        this.WritableBitmapGraphics = null;
                        this.WritableBitmap = null;
                        GC.Collect();
                    }
                    break;
#endif
                case RenderingMode.Default:
                    break;
                
                default:
                    break;
            }

            Initialize();
            if (!isIntialized)
                GenerateConatiners();
            else
                UpdateContainers();            

            if (this.XAxis != null)
            {
                this.XAxis.CalculateIntervalFromSeriesPoints();
                this.XAxis.Refresh();
            }
            if (this.YAxis != null)
            {
                this.YAxis.CalculateIntervalFromSeriesPoints();
                this.YAxis.Refresh();
            }
           
            foreach (SeriesBase series in this.Series)
            {
                if (Containers.Count > 0)
                    series.seriesContainer = Containers[series.Index];
                series.Refresh();
            }
           
        }
       
        public void Clear()
        {
            try
            {
                switch (this.RenderingMode)
                {
#if WPF
                    case RenderingMode.GDIRendering:
                        GDIGraphics.Clear(System.Windows.Media.Colors.Transparent.AsDrawingColor());
                        break;
                    case RenderingMode.WritableBitmap:
                        WritableBitmapGraphics.Clear(System.Windows.Media.Colors.Transparent.AsDrawingColor());
                        break;
#endif
                    case RenderingMode.Default:
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
#if WPF
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
            {
                Draw();
            }));
#elif WINRT
           IAsyncAction action= this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
            {
                Draw();
            });
#else

            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                Draw();
            }));
#endif
            
        }
        public virtual void Draw()
        {
        }
 #if WPF
        public void DrawPath(System.Windows.Media.PathGeometry seriesData, System.Drawing.Pen gdiPen, System.Drawing.Brush gdiBrush)
        {
            foreach (PathFigure figure in seriesData.Figures)
            {
                var points = figure.Segments.Cast<LineSegment>().Select(s => s.Point.AsDrawingPoint());

                var nextPoint = points.First();

                foreach (var point in points)
                {
                    var lastPoint = nextPoint;
                    nextPoint = point;
                    GDIGraphics.DrawLine(gdiPen, lastPoint, nextPoint);
                }
            }
        }
#endif
        internal void Initialize()
        {
            if (ActualHeight > 1 && ActualWidth > 1)
            {
                try
                {
#if WPF
                    System.Windows.Media.Matrix m = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
                    dpiFactor = 1 / m.M11;
#endif

                    switch (RenderingMode)
                    {
#if WPF
                        case RenderingMode.GDIRendering:
                            uint byteCount = (uint)(ActualWidth * ActualHeight * bpp);

                            //Allocate and create the InteropBitmap
                            var fileMappingPointer = CreateFileMapping(new IntPtr(-1), IntPtr.Zero, PAGE_READWRITE, 0, byteCount, null);
                            this.MapViewPointer = MapViewOfFile(fileMappingPointer, FILE_MAP_ALL_ACCESS, 0, 0, byteCount);
                            var format = PixelFormats.Bgra32;
                            var stride = (int)((int)ActualWidth * (int)format.BitsPerPixel / 8);
                            this.InteropBitmap = Imaging.CreateBitmapSourceFromMemorySection(fileMappingPointer,
                                                                                        (int)ActualWidth,
                                                                                        (int)ActualHeight,
                                                                                        format,
                                                                                        stride,
                                                                                        0) as InteropBitmap;
                            this.GDIGraphics = GetGdiGraphics(MapViewPointer);
                            break;  
          
                        case RenderingMode.WritableBitmap:
                            //Allocate and create the WritableBitmap
                            WritableBitmap = new WriteableBitmap((int)ActualWidth, (int)ActualHeight, 96, 96, PixelFormats.Bgra32, null);
                            ImageBitmap = new Bitmap((int)ActualWidth, (int)ActualHeight, ((int)ActualWidth * 4), System.Drawing.Imaging.PixelFormat.Format32bppPArgb, WritableBitmap.BackBuffer);
                            WritableBitmapGraphics = System.Drawing.Graphics.FromImage(ImageBitmap);
                            WritableBitmapGraphics.CompositingMode = this.CompositingMode.AsDrawingCompositingMode();
                            WritableBitmapGraphics.CompositingQuality = this.CompositingQuality.AsDrawingCompositingQuality();
                            WritableBitmapGraphics.SmoothingMode = this.SmoothingMode.AsDrawingSmoothingMode();
                            break;
#endif
                        default:
                            break;
                    }
                    
                    Clear();
                    this.IsBitmapInitialized = true;
                }
#if WPF
                catch (OutOfMemoryException)
                {
                    throw new OutOfMemoryException();
                }
#endif
                catch (Exception e)
                {
                    Debug.WriteLine("Exception Occured : " + e.Message);
                }

            }

        }

        public void InvalidateBitmap()
        {      
 #if WPF
            switch (RenderingMode)
            {
                case RenderingMode.Default:
                    break;
#if DIRECTX2D
                case RenderingMode.DirectX2D:
                    if (!isDirectXInitialized)
                        InitializeDirectX();
                    this.bitmapImage.Source = Directx2DGraphics;
                    break;
#endif

                case RenderingMode.GDIRendering:
                    this.bitmapImage.Source = (BitmapSource)InteropBitmap.GetAsFrozen();
                    break;
                case RenderingMode.WritableBitmap:
                    this.bitmapImage.Source = (BitmapSource)WritableBitmap.GetAsFrozen();
                    break;
                default:
                    break;
            }
            this.bitmapImage.InvalidateVisual();  
#endif
        }
        private int index;
        internal int Index
        {
            get { return index; }
            set { index = value; }
        }
#if WPF
        private Graphics GetGdiGraphics(IntPtr mapViewPointer)
        {
            Graphics gdiGraphics;
            //create the GDI Bitmap pointing to the same part of memory.
            System.Drawing.Bitmap gdiBitmap;
            gdiBitmap = new System.Drawing.Bitmap((int)ActualWidth,
                                                  (int)ActualHeight,
                                                  (int)ActualWidth * bpp,
                                                  System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                                  mapViewPointer);          
            // Get GDI Graphics 
            gdiGraphics = System.Drawing.Graphics.FromImage(gdiBitmap);
          
            gdiGraphics.CompositingMode = this.CompositingMode.AsDrawingCompositingMode();
            gdiGraphics.CompositingQuality = this.CompositingQuality.AsDrawingCompositingQuality();
            gdiGraphics.SmoothingMode = this.SmoothingMode.AsDrawingSmoothingMode();

            return gdiGraphics;
        }
#endif

        protected override Size MeasureOverride(Size constraint)
        {
            Size desiredSize = new Size(0, 0);
            int count = 0;

            foreach (UIElement child in Children)
            {
                //child.   
                Canvas.SetZIndex(child, count);
                count++;
                child.Measure(constraint);
                desiredSize.Width += child.DesiredSize.Width;
                desiredSize.Height += child.DesiredSize.Height;
            }
            if (Double.IsInfinity(constraint.Height))
                constraint.Height = desiredSize.Height;
            if (Double.IsInfinity(constraint.Width))
                constraint.Width = desiredSize.Width;
            return constraint;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (UIElement child in Children)
            {
                child.Arrange(new Rect(new Point(0, 0), arrangeSize));
            }           
            return arrangeSize;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "5"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass"), DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFileMapping(IntPtr hFile,
                                                       IntPtr lpFileMappingAttributes,
                                                       uint flProtect,
                                                       uint dwMaximumSizeHigh,
                                                       uint dwMaximumSizeLow,
                                                       string lpName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "4"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass"), DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject,
                                                   uint dwDesiredAccess,
                                                   uint dwFileOffsetHigh,
                                                   uint dwFileOffsetLow,
                                                   uint dwNumberOfBytesToMap);
        private void UpdateContainers()
        {
            axisLinesconatiner.Height = this.ActualHeight;
            axisLinesconatiner.Width = this.ActualWidth;            
            foreach (var container in Containers)
            {               
                container.RenderingMode = this.RenderingMode;
#if WPF
                container.dpiFactor = this.dpiFactor;
#endif
                //container.Height = this.ActualHeight;
                //container.Width = this.ActualWidth;
                switch (RenderingMode)
                {
#if WPF
                    case RenderingMode.GDIRendering:
                        container.InteropBitmap = this.InteropBitmap;
                        container.GDIGraphics = this.GDIGraphics;
                        break;
                    case RenderingMode.Default:
                        break;
                   case RenderingMode.WritableBitmap:
                        container.WritableBitmap = this.WritableBitmap;
                        container.WritableBitmapGraphics = this.WritableBitmapGraphics;
                        container.ImageBitmap = this.ImageBitmap;
                        break;
#endif
#if DIRECTX2D
                    case Chart.RenderingMode.DirectX2D:
                        container.Directx2DGraphics = this.Directx2DGraphics;
                        container.RenderTarget = this.RenderTarget;
                        break;
#endif

                    default:
                        break;
                }
            }
        }
        private void GenerateConatiners()
        {
            this.Children.Clear();
            this.Containers.Clear();
            axisLinesconatiner = new AxisCrossLinesContainer();
            Binding xAxisBinding = new Binding();
            xAxisBinding.Path = new PropertyPath("XAxis");
            xAxisBinding.Source = this;
            Binding yAxisBinding = new Binding();
            yAxisBinding.Path = new PropertyPath("YAxis");
            yAxisBinding.Source = this;
            axisLinesconatiner.SetBinding(AxisCrossLinesContainer.XAxisProperty, xAxisBinding);
            axisLinesconatiner.SetBinding(AxisCrossLinesContainer.YAxisProperty, yAxisBinding);
            axisLinesconatiner.Height = this.ActualHeight;
            axisLinesconatiner.Width = this.ActualWidth;            
            foreach (var seriesBase in Series)
            {
                SeriesContainer container = seriesBase.CreateContainer();
                seriesBase.Height = this.ActualHeight;
                seriesBase.Width = this.ActualWidth;
                container.Series = seriesBase;
                container.Container = this;
                container.RenderingMode = this.RenderingMode;
#if WPF
                container.dpiFactor = this.dpiFactor;      
#endif
                container.collection = this;
                switch (RenderingMode)
                {
#if WPF
                    case RenderingMode.GDIRendering:
                        container.InteropBitmap = this.InteropBitmap;
                        container.GDIGraphics = this.GDIGraphics;
                        break;
                   case RenderingMode.WritableBitmap:
                        container.WritableBitmap = this.WritableBitmap;
                        container.WritableBitmapGraphics = this.WritableBitmapGraphics;
                        container.ImageBitmap = this.ImageBitmap;
                        break;
#endif
                    case RenderingMode.Default:
                        this.Children.Add(container.PartsCanvas);
                        break;
#if DIRECTX2D
                    case Chart.RenderingMode.DirectX2D:
                        container.Directx2DGraphics = this.Directx2DGraphics;
                        container.RenderTarget = this.RenderTarget;
                        break;
#endif
                   
                    default:
                        break;
                }
                                
                this.Containers.Add(container);
            }
                    
            this.Children.Add(bitmapImage);
            this.Children.Add(axisLinesconatiner);   
            
            isIntialized = true;
        }
#if WPF
        internal SmoothingMode SmoothingMode
        {
            get { return (SmoothingMode)GetValue(SmoothingModeProperty); }
            set { SetValue(SmoothingModeProperty, value); }
        }

        public static readonly DependencyProperty SmoothingModeProperty =
            DependencyProperty.Register("SmoothingMode", typeof(SmoothingMode), typeof(ContainerCollection), new PropertyMetadata(SmoothingMode.HighQuality));



        internal CompositingQuality CompositingQuality
        {
            get { return (CompositingQuality)GetValue(CompositingQualityProperty); }
            set { SetValue(CompositingQualityProperty, value); }
        }

        public static readonly DependencyProperty CompositingQualityProperty =
            DependencyProperty.Register("CompositingQuality", typeof(CompositingQuality), typeof(ContainerCollection), new PropertyMetadata(CompositingQuality.HighQuality));

         internal CompositingMode CompositingMode
        {
            get { return (CompositingMode)GetValue(CompositingModeProperty); }
            set { SetValue(CompositingModeProperty, value); }
        }

        public static readonly DependencyProperty CompositingModeProperty =
            DependencyProperty.Register("CompositingMode", typeof(CompositingMode), typeof(ContainerCollection), new PropertyMetadata(CompositingMode.SourceOver));

#endif
        public RenderingMode RenderingMode
        {
            get { return (RenderingMode)GetValue(RenderingModeProperty); }
            set { SetValue(RenderingModeProperty, value); }
        }

        public static readonly DependencyProperty RenderingModeProperty =
            DependencyProperty.Register("RenderingMode", typeof(RenderingMode), typeof(ContainerCollection), new PropertyMetadata(RenderingMode.Default));



        public SparrowChart Chart
        {
            get { return (SparrowChart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        public static readonly DependencyProperty ChartProperty =
            DependencyProperty.Register("Chart", typeof(SparrowChart), typeof(ContainerCollection), new PropertyMetadata(null));



        public XAxis XAxis
        {
            get { return (XAxis)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        public static readonly DependencyProperty XAxisProperty =
            DependencyProperty.Register("XAxis", typeof(XAxis), typeof(ContainerCollection), new PropertyMetadata(null));

        public YAxis YAxis
        {
            get { return (YAxis)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }

        public static readonly DependencyProperty YAxisProperty =
            DependencyProperty.Register("YAxis", typeof(YAxis), typeof(ContainerCollection), new PropertyMetadata(null));
#if DIRECTX2D
        public Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SolidColorBrush GetDirectXBrush(Color color)
        {
            Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SolidColorBrush brush = this.RenderTarget.CreateSolidColorBrush(new ColorF(((float)color.R / 255), ((float)color.G / 255), ((float)color.B / 255), ((float)color.A / 255)));
            return brush;
        }

        public Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush GetDirectXBrush(System.Windows.Media.Brush mediaBrush)
        {
            if (mediaBrush is System.Windows.Media.SolidColorBrush)
                return GetDirectXBrush(((System.Windows.Media.SolidColorBrush)mediaBrush).Color);
            else if (mediaBrush is System.Windows.Media.RadialGradientBrush)
            {
                System.Windows.Media.RadialGradientBrush mediaRadialBrush = (mediaBrush as System.Windows.Media.RadialGradientBrush);
                RadialGradientBrushProperties properties = new RadialGradientBrushProperties(mediaRadialBrush.Center.AsDirectX2DPoint(), mediaRadialBrush.GradientOrigin.AsDirectX2DPoint(), (float)mediaRadialBrush.RadiusX, (float)mediaRadialBrush.RadiusY);
                List<Microsoft.WindowsAPICodePack.DirectX.Direct2D1.GradientStop> stopCollection = new List<Microsoft.WindowsAPICodePack.DirectX.Direct2D1.GradientStop>();
                foreach (var stop in mediaRadialBrush.GradientStops)
                {
                    stopCollection.Add(new Microsoft.WindowsAPICodePack.DirectX.Direct2D1.GradientStop((float)stop.Offset, new ColorF((float)stop.Color.R, ((float)stop.Color.G / 255), ((float)stop.Color.B / 255), ((float)stop.Color.A / 255))));
                }
                Microsoft.WindowsAPICodePack.DirectX.Direct2D1.GradientStopCollection stops = this.RenderTarget.CreateGradientStopCollection(stopCollection, Gamma.StandardRgb, ExtendMode.Wrap);

                Microsoft.WindowsAPICodePack.DirectX.Direct2D1.RadialGradientBrush radialBrush = this.RenderTarget.CreateRadialGradientBrush(properties, stops);
                return radialBrush;
            }
            return null;

        }

        /// <summary>
        /// Raised when the content of the Scene has changed.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>Gets the surface this instance draws to.</summary>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Dispose()"/> has been called on this instance.
        /// </exception>
        public D3D10.Texture2D Texture
        {
            get
            {
                this.ThrowIfDisposed();
                return this.texture;
            }
        }

        /// <summary>
        /// Gets the <see cref="D2D.D2DFactory"/> used to create the resources.
        /// </summary>
        protected D2D.D2DFactory Factory
        {
            get { return this.factory; }
        }


        /// <summary>
        /// Gets the <see cref="D2D.RenderTarget"/> used for drawing.
        /// </summary>
        protected D2D.RenderTarget RenderTarget
        {
            get { return this.renderTarget; }
        }

        /// <summary>
        /// Immediately frees any system resources that the object might hold.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates a DirectX 10 device and related device specific resources.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// A previous call to CreateResources has not been followed by a call to
        /// <see cref="FreeResources"/>.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Dispose()"/> has been called on this instance.
        /// </exception>
        /// <exception cref="DirectX.DirectXException">
        /// Unable to create a DirectX 10 device or an error occured creating
        /// device dependent resources.
        /// </exception>
        public void CreateResources()
        {
            FreeResources();
            this.ThrowIfDisposed();
            if (this.device != null)
            {
                throw new InvalidOperationException("A previous call to CreateResources has not been followed by a call to FreeResources.");
            }

            // Try to create a hardware device first and fall back to a
            // software (WARP doens't let us share resources)
            var device1 = TryCreateDevice1(D3D10.DriverType.Hardware);
            if (device1 == null)
            {
                device1 = TryCreateDevice1(D3D10.DriverType.Software);
                if (device1 == null)
                {
                    throw new DirectX.DirectXException("Unable to create a DirectX 10 device.");
                }
            }
            this.device = device1.QueryInterface<D3D10.D3DDevice>();
            device1.Dispose();
        }

        /// <summary>
        /// Releases the DirectX device and any device dependent resources.
        /// </summary>
        /// <remarks>
        /// This method is safe to be called even if the instance has been disposed.
        /// </remarks>
        public void FreeResources()
        {
            this.OnFreeResources();

            if (this.texture != null)
            {
                this.texture.Dispose();
                this.texture = null;
            }
            if (this.renderTarget != null)
            {
                this.renderTarget.Dispose();
                this.renderTarget = null;
            }
            if (this.device != null)
            {
                this.device.Dispose();
                this.device = null;
            }
        }

        /// <summary>
        /// Causes the scene to redraw its contents.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Resize"/> has not been called.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Dispose()"/> has been called on this instance.
        /// </exception>
        public void Render()
        {
            this.ThrowIfDisposed();
            if (this.renderTarget == null)
            {
                throw new InvalidOperationException("Resize has not been called.");
            }

            this.OnRender();
            this.device.Flush();
            this.OnUpdated();
        }

        /// <summary>Resizes the scene.</summary>
        /// <param name="width">The new width for the scene.</param>
        /// <param name="height">The new height for the scene.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// width/height is less than zero.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <see cref="CreateResources"/> has not been called.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Dispose()"/> has been called on this instance.
        /// </exception>
        /// <exception cref="DirectX.DirectXException">
        /// An error occured creating device dependent resources.
        /// </exception>
        public void Resize(int width, int height)
        {
            this.ThrowIfDisposed();
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException("width", "Value must be positive.");
            }
            if (height < 0)
            {
                throw new ArgumentOutOfRangeException("height", "Value must be positive.");
            }
            if (this.device == null)
            {
                throw new InvalidOperationException("CreateResources has not been called.");
            }

            // Recreate the render target
            this.CreateTexture(width, height);
            using (var surface = this.texture.QueryInterface<DirectX.Graphics.Surface>())
            {
                this.CreateRenderTarget(surface);
            }

            // Resize our viewport
            var viewport = new D3D10.Viewport();
            viewport.Height = (uint)height;
            viewport.MaxDepth = 1;
            viewport.MinDepth = 0;
            viewport.TopLeftX = 0;
            viewport.TopLeftY = 0;
            viewport.Width = (uint)width;
            this.device.RS.Viewports = new D3D10.Viewport[] { viewport };

            // Destroy and recreate any dependent resources declared in a
            // derived class only (i.e don't destroy our resources).
            this.OnFreeResources();
            this.OnCreateResources();
        }

        /// <summary>
        /// Immediately frees any system resources that the object might hold.
        /// </summary>
        /// <param name="disposing">
        /// Set to true if called from an explicit disposer; otherwise, false.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            this.FreeResources();
            if (disposing)
            {
                this.factory.Dispose();
            }

            this.disposed = true;
        }

        /// <summary>
        /// When overriden in a derived class, creates device dependent resources.
        /// </summary>
        internal void OnCreateResources()
        {
        }

        /// <summary>
        /// When overriden in a deriven class, releases device dependent resources.
        /// </summary>
        internal void OnFreeResources()
        {
        }

        /// <summary>
        /// When overriden in a derived class, renders the Direct2D content.
        /// </summary>
        protected virtual void OnRender()
        {
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if
        /// <see cref="Dispose()"/> has been called on this instance.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        private static D3D10.D3DDevice1 TryCreateDevice1(D3D10.DriverType type)
        {
            // We'll try to create the device that supports any of these feature levels
            DirectX.Direct3D.FeatureLevel[] levels =
            {
                DirectX.Direct3D.FeatureLevel.Ten,
                DirectX.Direct3D.FeatureLevel.NinePointThree,
                DirectX.Direct3D.FeatureLevel.NinePointTwo,
                DirectX.Direct3D.FeatureLevel.NinePointOne
            };

            foreach (var level in levels)
            {
                try
                {
                    return D3D10.D3DDevice1.CreateDevice1(null, type, null, D3D10.CreateDeviceOptions.SupportBgra, level);
                }
                catch (ArgumentException) // E_INVALIDARG
                {
                    continue; // Try the next feature level
                }
                catch (OutOfMemoryException) // E_OUTOFMEMORY
                {
                    continue; // Try the next feature level
                }
                catch (DirectX.DirectXException) // D3DERR_INVALIDCALL or E_FAIL
                {
                    continue; // Try the next feature level
                }
            }
            return null; // We failed to create a device at any required feature level
        }

        private void CreateRenderTarget(DirectX.Graphics.Surface surface)
        {
            // Create a D2D render target which can draw into our offscreen D3D
            // surface. D2D uses device independant units, like WPF, at 96/inch
            var properties = new D2D.RenderTargetProperties();
            properties.DpiX = 96;
            properties.DpiY = 96;
            properties.MinLevel = DirectX.Direct3D.FeatureLevel.Default;
            properties.PixelFormat = new D2D.PixelFormat(DirectX.Graphics.Format.Unknown, D2D.AlphaMode.Premultiplied);
            properties.RenderTargetType = D2D.RenderTargetType.Default;
            properties.Usage = D2D.RenderTargetUsages.None;           
            // Assign result to temporary variable in case CreateGraphicsSurfaceRenderTarget throws
            var target = this.factory.CreateGraphicsSurfaceRenderTarget(surface, properties);
            target.AntiAliasMode = AntiAliasMode.Aliased;
            if (this.renderTarget != null)
            {
                this.renderTarget.Dispose();
            }
            this.renderTarget = target;
        }

        private void CreateTexture(int width, int height)
        {
            var description = new D3D10.Texture2DDescription();
            description.ArraySize = 1;
            description.BindingOptions = D3D10.BindingOptions.RenderTarget | D3D10.BindingOptions.ShaderResource;
            description.CpuAccessOptions = D3D10.CpuAccessOptions.None;
            description.Format = DirectX.Graphics.Format.B8G8R8A8UNorm;
            description.MipLevels = 1;
            description.MiscellaneousResourceOptions = D3D10.MiscellaneousResourceOptions.Shared;
            description.SampleDescription = new DirectX.Graphics.SampleDescription(1, 0);
            description.Usage = D3D10.Usage.Default;

            description.Height = (uint)height;
            description.Width = (uint)width;

            // Assign result to temporary variable in case CreateTexture2D throws
            var texture = this.device.CreateTexture2D(description);
            if (this.texture != null)
            {
                this.texture.Dispose();
            }
            this.texture = texture;
        }

        private void OnUpdated()
        {
            var callback = this.Updated;
            if (callback != null)
            {
                callback(this, EventArgs.Empty);
            }
        }
#endif
    }
}
