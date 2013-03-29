using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Data;
using System.Windows.Media;
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
    /// ScatterSeries for SparrowChart
    /// </summary>
    public class ScatterSeries : FillSeriesBase
    {
        public ScatterSeries()
        {
            ScatterPoints = new PointCollection();
            isFill = true;
        }

        public override void GenerateDatas()
        {
            ScatterPoints.Clear();
            Parts.Clear();
            Point endPoint = new Point(0, 0);
            Point startPoint = new Point(0, 0);
            int index = 0;
            if (this.Points != null && this.seriesContainer != null && this.Points.Count > 1)
            {
                CalculateMinAndMax();
                ChartPoint oldPoint = new ChartPoint() { XValue = 0, YValue = 0 };
                IntializePoints();                
                foreach (ChartPoint point in this.Points)
                {
                    if (CheckValuePoint(oldPoint, point))
                    {
                        Point linePoint = NormalizePoint(new Point(point.XValue, point.YValue));                       
                        ScatterPoints.Add(linePoint);
                        oldPoint = point;
                    }
                }
                if (this.RenderingMode == RenderingMode.Default)
                    for (int i = 0; i < ScatterPoints.Count; i++)
                    {
                        ScatterPart scatterPart = new ScatterPart(ScatterPoints[i]);
                        Binding sizeBinding = new Binding();
                        sizeBinding.Path = new PropertyPath("ScatterSize");
                        sizeBinding.Source = this;
                        scatterPart.SetBinding(ScatterPart.SizeProperty, sizeBinding);
                        SetBindingForStrokeandStrokeThickness(scatterPart);
                        this.Parts.Add(scatterPart);
                    }
                
                if (this.seriesContainer != null)
                    this.seriesContainer.Invalidate();
            }
            isRefreshed = false;
        }

        internal override SeriesContainer CreateContainer()
        {
            return new ScatterContainer();
        }

        public PointCollection ScatterPoints
        {
            get { return (PointCollection)GetValue(ScatterPointsProperty); }
            set { SetValue(ScatterPointsProperty, value); }
        }

        public static readonly DependencyProperty ScatterPointsProperty =
            DependencyProperty.Register("ScatterPoints", typeof(PointCollection), typeof(ScatterSeries), new PropertyMetadata(null));


        public double ScatterSize
        {
            get { return (double)GetValue(ScatterSizeProperty); }
            set { SetValue(ScatterSizeProperty, value); }
        }

        public static readonly DependencyProperty ScatterSizeProperty =
            DependencyProperty.Register("ScatterSize", typeof(double), typeof(ScatterSeries), new PropertyMetadata(30d));


           
    }
}
