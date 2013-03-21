﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sparrow.Chart
{
    /// <summary>
    /// SparrowChart Control
    /// </summary>
    [ContentProperty("Series")]
    public class SparrowChart : Control 
    {
        static SparrowChart()
        {            
        }

        internal ContainerCollection containers;
        internal int indexCount = 0;
        bool isMouseDragging;
        bool isMouseClick;
        internal DockPanel rootDockPanel;
        ResourceDictionary styles;

        private static List<string> actualCategoryValues;
        internal static List<string> ActualCategoryValues
        {
            get { return actualCategoryValues; }
            set { actualCategoryValues = value; }
        }

        public override void OnApplyTemplate()
        {            
            containers = (ContainerCollection)this.GetTemplateChild("PART_containers");
            rootDockPanel = (DockPanel)this.GetTemplateChild("Part_rootDockPanel");
            containers.MouseMove += OnMouseMove;
            containers.MouseLeave += OnMouseLeave;
            containers.MouseLeftButtonDown += OnMousePress;
            containers.MouseLeftButtonUp += OnMouseRelease;
            containers.ClipToBounds = true;            
            BrushTheme();       
            base.OnApplyTemplate();
        }

        void OnMouseRelease(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        void OnMousePress(object sender, System.Windows.Input.MouseEventArgs e)
        {                         
           
        }
        void OnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }
        void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }              

        public SparrowChart()
        {
            this.DefaultStyleKey = typeof(SparrowChart);
            this.Series = new SeriesCollection();
            this.Series.CollectionChanged += OnSeriesCollectionChanged;
            brushes = Themes.MetroBrushes();
            ActualCategoryValues = new List<string>();
            styles = new ResourceDictionary()
            {
#if X86
                Source = new Uri(@"/Sparrow.Chart.DirectX2D_x86;component/Themes/Styles.xaml", UriKind.Relative)
#elif X64
                Source = new Uri(@"/Sparrow.Chart.DirectX2D_x64;component/Themes/Styles.xaml", UriKind.Relative)
#else
                Source = new Uri(@"/Sparrow.Chart;component/Themes/Styles.xaml", UriKind.Relative)
#endif
            };
            this.ContainerBorderStyle = styles["containerBorderStyle"] as Style;
        }

        void OnSeriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    
                    foreach (SeriesBase series in e.NewItems)
                    {
                        Binding dataContextBinding = new Binding("DataContext");
                        dataContextBinding.Source = this;
                        BindingOperations.SetBinding(series, SeriesBase.DataContextProperty, dataContextBinding);                       
                        Binding renderingModeBinding = new Binding("RenderingMode");
                        renderingModeBinding.Source = this;
                        BindingOperations.SetBinding(series, SeriesBase.RenderingModeProperty, renderingModeBinding);
                        series.Chart = this;
                        series.Index = indexCount;
                        series.XAxis = XAxis;
                        series.YAxis = YAxis;                       
                        indexCount++;
                    }                   
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
                    
            }
            
        }

        /// <summary>
        /// Export the Sparrow Chart as Image
        /// </summary>
        /// <param name="fileName">Output File Name to Export the Chart as Image</param>
        public void ExportAsImage(string fileName)
        {
            string fileExtension = new FileInfo(fileName).Extension.ToLower(CultureInfo.InvariantCulture);

            BitmapEncoder imageEncoder = null;
            switch (fileExtension)
            {
                case ".bmp":
                    imageEncoder = new BmpBitmapEncoder();
                    break;

                case ".jpg":
                case ".jpeg":
                    imageEncoder = new JpegBitmapEncoder();
                    break;

                case ".png":
                    imageEncoder = new PngBitmapEncoder();
                    break;

                case ".gif":
                    imageEncoder = new GifBitmapEncoder();
                    break;

                case ".tif":
                case ".tiff":
                    imageEncoder = new TiffBitmapEncoder();
                    break;

                case ".wdp":
                    imageEncoder = new WmpBitmapEncoder();
                    break;

                default:
                    imageEncoder = new BmpBitmapEncoder();
                    break;
            }

            RenderTargetBitmap bmpSource =new RenderTargetBitmap((int)this.ActualWidth,(int)this.ActualHeight, 96, 96,PixelFormats.Pbgra32);
            bmpSource.Render(this);

            imageEncoder.Frames.Add(BitmapFrame.Create(bmpSource));
            using (Stream stream = File.Create(fileName))
            {
                imageEncoder.Save(stream);
                stream.Close();
            }
            
        }

        /// <summary>
        /// Export the Sparrow Chart as Image
        /// </summary>
        /// <param name="fileName">Output File Name to Export the Chart as Image<</param>
        /// <param name="imageEncoder">Image Encoder Format for output image<</param>
        public void ExportAsImage(string fileName,BitmapEncoder imageEncoder)
        {
            string fileExtension = new FileInfo(fileName).Extension.ToLower(CultureInfo.InvariantCulture);                  

            RenderTargetBitmap bmpSource = new RenderTargetBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bmpSource.Render(this);

            imageEncoder.Frames.Add(BitmapFrame.Create(bmpSource));
            using (Stream stream = File.Create(fileName))
            {
                imageEncoder.Save(stream);
                stream.Close();
            }
        }

        private void BrushTheme()
        {
            if (this.Series != null)
                foreach (var series in Series)
                {
                    if (series.Stroke == null)
                    {
                        if (brushes.Length > 1)
                            series.Stroke = brushes[series.Index % (brushes.Length - 1)];
                        else
                            series.Stroke = brushes[brushes.Length - 1];
                    }
                    if (series.isFill && (series as FillSeriesBase).Fill == null)
                    {
                        if (brushes.Length > 1)
                            (series as FillSeriesBase).Fill = brushes[series.Index % (brushes.Length - 1)];
                        else
                            (series as FillSeriesBase).Fill = brushes[brushes.Length - 1];
                    }
                }
        }

        protected override Size MeasureOverride(Size constraint)
        {                                
            if (rootDockPanel != null)
            {                
                rootDockPanel.Measure(constraint);               
            }
           
            return base.MeasureOverride(constraint);
        }


        public XAxis XAxis
        {
            get { return (XAxis)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }
        
        public static readonly DependencyProperty XAxisProperty =
            DependencyProperty.Register("XAxis", typeof(XAxis), typeof(SparrowChart), new PropertyMetadata(null,OnXAxisChanged));

        private static void OnXAxisChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as SparrowChart).XAxisChanged(args);
        }
        internal void XAxisChanged(DependencyPropertyChangedEventArgs args)
        {
            Binding seriesBinding = new Binding("Series");
            seriesBinding.Source = this;
            this.XAxis.SetBinding(AxisBase.SeriesProperty, seriesBinding);
            this.XAxis.Chart = this;
        }

        public YAxis YAxis
        {
            get { return (YAxis)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }
        
        public static readonly DependencyProperty YAxisProperty =
            DependencyProperty.Register("YAxis", typeof(YAxis), typeof(SparrowChart), new PropertyMetadata(null,OnYAxisChanged));

        private static void OnYAxisChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as SparrowChart).YAxisChanged(args);
        }
        internal void YAxisChanged(DependencyPropertyChangedEventArgs args)
        {
            Binding seriesBinding = new Binding("Series");
            seriesBinding.Source = this;
            this.YAxis.SetBinding(AxisBase.SeriesProperty, seriesBinding);
            this.YAxis.Chart = this;
        }
        public Double AxisHeight
        {
            get { return (Double)GetValue(AxisHeightProperty); }
            set { SetValue(AxisHeightProperty, value); }
        }

        
        public static readonly DependencyProperty AxisHeightProperty =
            DependencyProperty.Register("AxisHeight", typeof(Double), typeof(SparrowChart), new PropertyMetadata(30d));



        public Double AxisWidth
        {
            get { return (Double)GetValue(AxisWidthProperty); }
            set { SetValue(AxisWidthProperty, value); }
        }
        
        public static readonly DependencyProperty AxisWidthProperty =
            DependencyProperty.Register("AxisWidth", typeof(Double), typeof(SparrowChart), new PropertyMetadata(30d));



        public bool IsRefresh
        {
            get { return (bool)GetValue(IsRefreshProperty); }
            set { SetValue(IsRefreshProperty, value); }
        }

        public static readonly DependencyProperty IsRefreshProperty =
            DependencyProperty.Register("IsRefresh", typeof(bool), typeof(SparrowChart), new PropertyMetadata(true,OnIsRefreshChanged));

        private static void OnIsRefreshChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SparrowChart sender = obj as SparrowChart;
            sender.IsRefreshChanged(args);
        }
        private void IsRefreshChanged(DependencyPropertyChangedEventArgs args)
        {
            foreach (SeriesBase series in Series)
            {
                series.IsRefresh = (bool)args.NewValue;
                if ((bool)args.NewValue)
                    series.Refresh();
            }           

        }

        public SeriesCollection Series
        {
            get { return (SeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series", typeof(SeriesCollection), typeof(SparrowChart), new PropertyMetadata(null));


        public Legend Legend
        {
            get { return (Legend)GetValue(LegendProperty); }
            set { SetValue(LegendProperty, value); }
        }

        public static readonly DependencyProperty LegendProperty =
            DependencyProperty.Register("Legend", typeof(Legend), typeof(SparrowChart), new PropertyMetadata(null,OnLegendChanged));

        private static void OnLegendChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as SparrowChart).LegendChanged(args);
        }
        internal void LegendChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.Legend != null)
            {
                this.Legend.Chart = this;              
            }
        }

        internal Dock LegendDock
        {
            get { return (Dock)GetValue(LegendDockProperty); }
            set { SetValue(LegendDockProperty, value); }
        }

        public static readonly DependencyProperty LegendDockProperty =
            DependencyProperty.Register("LegendDock", typeof(Dock), typeof(SparrowChart), new PropertyMetadata(Dock.Top));


        public Theme Theme
        {
            get { return (Theme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register("Theme", typeof(Theme), typeof(SparrowChart), new PropertyMetadata(Theme.Metro,OnThemeChanged));

        private static void OnThemeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as SparrowChart).ThemeChanged(args);
        }

        internal void ThemeChanged(DependencyPropertyChangedEventArgs args)
        {
            switch (Theme)
            {
                case Theme.Metro:
                    brushes = Themes.MetroBrushes();
                    break;                               
                case Theme.Custom:
                    break;
                default:
                    break;
            }           
           
            BrushTheme();
        }

        

        private Brush[] brushes;

        public Brush[] Brushes
        {
            get { return (Brush[])GetValue(BrushesProperty); }
            set { SetValue(BrushesProperty, value); }
        }

        public static readonly DependencyProperty BrushesProperty =
            DependencyProperty.Register("Brushes", typeof(Brush[]), typeof(SparrowChart), new PropertyMetadata(null));       

        public SmoothingMode SmoothingMode
        {
            get { return (SmoothingMode)GetValue(SmoothingModeProperty); }
            set { SetValue(SmoothingModeProperty, value); }
        }

        public static readonly DependencyProperty SmoothingModeProperty =
            DependencyProperty.Register("SmoothingMode", typeof(SmoothingMode), typeof(SparrowChart), new PropertyMetadata(SmoothingMode.HighQuality));
      

        public CompositingQuality CompositingQuality
        {
            get { return (CompositingQuality)GetValue(CompositingQualityProperty); }
            set { SetValue(CompositingQualityProperty, value); }
        }

        public static readonly DependencyProperty CompositingQualityProperty =
            DependencyProperty.Register("CompositingQuality", typeof(CompositingQuality), typeof(SparrowChart), new PropertyMetadata(CompositingQuality.HighQuality));



        public Style ContainerBorderStyle
        {
            get { return (Style)GetValue(ContainerBorderStyleProperty); }
            set { SetValue(ContainerBorderStyleProperty, value); }
        }

        public static readonly DependencyProperty ContainerBorderStyleProperty =
            DependencyProperty.Register("ContainerBorderStyle", typeof(Style), typeof(SparrowChart), new PropertyMetadata(null));


        public RenderingMode RenderingMode
        {
            get { return (RenderingMode)GetValue(RenderingModeProperty); }
            set { SetValue(RenderingModeProperty, value); }
        }

        public static readonly DependencyProperty RenderingModeProperty =
            DependencyProperty.Register("RenderingMode", typeof(RenderingMode), typeof(SparrowChart), new PropertyMetadata(RenderingMode.DefaultWPFRendering));

        public CompositingMode CompositingMode
        {
            get { return (CompositingMode)GetValue(CompositingModeProperty); }
            set { SetValue(CompositingModeProperty, value); }
        }

        public static readonly DependencyProperty CompositingModeProperty =
            DependencyProperty.Register("CompositingMode", typeof(CompositingMode), typeof(SparrowChart), new PropertyMetadata(CompositingMode.SourceOver));

    }
}
