using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sparrow.Chart
{
    public class SparrowChart : Control
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SparrowChart"/> class.
        /// </summary>
        public SparrowChart()
        {
            this.SetDefaultValues();
            this.GetBrushes();
        }
       
        internal RectF PositionedRect
        {
            get;
            set;
        }        

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Brush> Brushes
        {
            get;
            set;
        }

         [DefaultValue(false)]
        public SmoothingMode SmoothingMode
        {
            get;
            set;
        }

        [DefaultValue(false)]
        public CompositingQuality CompositingQuality
        {
            get; 
            set;
        }

         [DefaultValue(false)]
        public CompositingMode CompositingMode
        {
            get; 
            set; 
        }


        private SeriesCollection series;
        [DefaultValue(false)]
        public SeriesCollection Series
        {
            get { return series; }
            set
            {
                if(series != null)
                    series.CollectionChanged -= OnSeriesCollectionChanged;
                series = value;
                if(value != null)
                    series.CollectionChanged += OnSeriesCollectionChanged;
            }
        }

        private void OnSeriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                    case NotifyCollectionChangedAction.Add:
                    foreach (Series newSeries in e.NewItems)
                    {                        
                        if (newSeries.XAxis == null)
                            newSeries.XAxis = this.XAxis;
                        if (newSeries.YAxis == null)
                            newSeries.YAxis = this.YAxis;
                        if (Brushes.Count > 1)
                            newSeries.Stroke = newSeries.Stroke ?? Brushes[this.Series.IndexOf(newSeries) % (Brushes.Count)];
                        else
                            newSeries.Stroke = newSeries.Stroke ?? Brushes[Brushes.Count];                                              
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
                     
            }
            Invalidate();
        }


        internal Axes Axes
        {
            get;
            set;
        }

        [DefaultValue(false)]
        public Legend Legend
        {
            get; 
            set;
        }

        protected override void OnPaint(PaintEventArgs e)
        {            
            this.RootGraphics = e.Graphics;
            this.RootGraphics.CompositingMode = this.CompositingMode;
            this.RootGraphics.CompositingQuality = this.CompositingQuality;
            this.RootGraphics.SmoothingMode = this.SmoothingMode;
            
            Pen borderPen = this.BorderPen ?? new Pen(this.BorderBrush,this.BorderThickness);
            this.RootGraphics.DrawRectangle(borderPen, 0, 0, this.Width - (borderPen.Width), this.Height - (borderPen.Width));
            this.PositionedRect=new RectF(new PointF(0, 0), new SizeF(this.Width, this.Height));
            if (this.XAxis != null)
                this.XAxis.PositionedRect = this.PositionedRect;
            if (this.YAxis != null)
                this.YAxis.PositionedRect = this.PositionedRect;
            foreach (var series in Series)
            {
                series.RootGraphics = this.RootGraphics;
                series.Refresh();
            }
            
        }


        internal Graphics RootGraphics
        {
            get; 
            set;
        }

        private Theme theme;
        [DefaultValue(false)]
        public Theme Theme
        {
            get
            {
                return theme;
            }
            set 
            { 
                theme = value;
                GetBrushes();
                BrushTheme();
            }
        }

        [DefaultValue(false)]
        public XAxis XAxis
        {
            get; 
            set;
        }

         [DefaultValue(false)]
        public YAxis YAxis
        {
            get;
            set;
        }


        private void SetDefaultValues()
        {
            this.Series=new SeriesCollection();
            this.Theme=Theme.Metro;
            this.Axes=new Axes();
            this.SmoothingMode = SmoothingMode.HighQuality;
            this.CompositingMode=CompositingMode.SourceOver;
            this.CompositingQuality=CompositingQuality.HighQuality;
            this.Brushes=new List<Brush>();
            this.XAxis=new CategoryXAxis();
            this.YAxis = new LinearYAxis();
            this.BorderBrush=new SolidBrush(Color.Black);
            this.BorderThickness = 1.0f;
            this.ContainerBorderPen = new Pen(Color.Black, 0f);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
           Invalidate();
        }

        private void GetBrushes()
        {
            switch (Theme)
            {
                case Theme.Arctic:
                    Brushes = Themes.ArcticBrushes();
                    break;
                case Theme.Autmn:
                    Brushes = Themes.AutmnBrushes();
                    break;
                case Theme.Cold:
                    Brushes = Themes.ColdBrushes();
                    break;
                case Theme.Flower:
                    Brushes = Themes.FlowerBrushes();
                    break;
                case Theme.Forest:
                    Brushes = Themes.ForestBrushes();
                    break;
                case Theme.Grayscale:
                    Brushes = Themes.GrayscaleBrushes();
                    break;
                case Theme.Ground:
                    Brushes = Themes.GroundBrushes();
                    break;
                case Theme.Lialac:
                    Brushes = Themes.LialacBrushes();
                    break;
                case Theme.Natural:
                    Brushes = Themes.NaturalBrushes();
                    break;
                case Theme.Pastel:
                    Brushes = Themes.PastelBrushes();
                    break;
                case Theme.Rainbow:
                    Brushes = Themes.RainbowBrushes();
                    break;
                case Theme.Spring:
                    Brushes = Themes.SpringBrushes();
                    break;
                case Theme.Summer:
                    Brushes = Themes.SummerBrushes();
                    break;
                case Theme.Warm:
                    Brushes = Themes.WarmBrushes();
                    break;
                case Theme.Metro:
                    Brushes = Themes.MetroBrushes();
                    break;
                case Theme.Custom:
                    break;
            }    
        }
        private void BrushTheme()
        {
            if (this.Series != null)
                foreach (var series in Series)
                {
                    if (Brushes.Count > 1)
                        series.Stroke = Brushes[Series.IndexOf(series)%(Brushes.Count)];
                    else
                        series.Stroke = Brushes[Brushes.Count];
                }
        }

        [DefaultValue(false)]
        public Brush BorderBrush
        {
            get; 
            set;
        }

        [DefaultValue(false)]
        public float BorderThickness
        {
            get; 
            set;
        }

        [DefaultValue(false)]
        public Pen BorderPen
        {
            get; 
            set;
        }

        [DefaultValue(false)]
        public Pen ContainerBorderPen
        {
            get;
            set;
        }        
        
    }
}
