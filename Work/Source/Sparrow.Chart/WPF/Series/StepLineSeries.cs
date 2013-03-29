using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Media;
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
    /// StepLine Series for Sparrow Chart
    /// </summary>
    public class StepLineSeries : LineSeriesBase
    {

        public StepLineSeries()
        {            
            LinePoints = new PointCollection(); 
        }

        override public void GenerateDatas()
        {
            LinePoints.Clear();
            Parts.Clear();
            if (this.Points != null && this.seriesContainer != null)
            {
                CalculateMinAndMax();
                ChartPoint oldPoint = new ChartPoint() { XValue = 0, YValue = 0 };
                IntializePoints();
                for (int i = 0; i < this.Points.Count; i++)
                {
                    ChartPoint point = this.Points[i];
                    ChartPoint step = new ChartPoint();
                    if (!(i == this.Points.Count - 1))
                        step = this.Points[i + 1];
                    if (CheckValuePoint(oldPoint, point))
                    {
                        Point linePoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        LinePoints.Add(linePoint);
                        if (!(i == this.Points.Count - 1))
                        {
                            Point stepPoint = NormalizePoint(new Point(point.XValue, step.YValue));
                            LinePoints.Add(stepPoint);
                        }
                    }
                }
                if (this.RenderingMode == RenderingMode.Default)
                {
                    if (!this.UseSinglePart)
                    {
                        for (int i = 0; i < LinePoints.Count - 2; i++)
                        {
                            StepLinePart stepLinePart = new StepLinePart(LinePoints[i], LinePoints[i + 1], LinePoints[i + 2]);
                            SetBindingForStrokeandStrokeThickness(stepLinePart);
                            this.Parts.Add(stepLinePart);
                        }
                    }
                    else
                    {
                        LineSinglePart stepLinePart = new LineSinglePart();
                        stepLinePart.LinePoints = this.LinePoints;
                        SetBindingForStrokeandStrokeThickness(stepLinePart);
                        this.Parts.Add(stepLinePart);
                    }
                }
                if (this.seriesContainer != null)
                    this.seriesContainer.Invalidate();
            }
            isRefreshed = false;
        }

        internal override SeriesContainer CreateContainer()
        {
            return new StepLineContainer();
        }
        public PointCollection LinePoints
        {
            get { return (PointCollection)GetValue(LinePointsProperty); }
            set { SetValue(LinePointsProperty, value); }
        }

        public static readonly DependencyProperty LinePointsProperty =
            DependencyProperty.Register("LinePoints", typeof(PointCollection), typeof(StepLineSeries), new PropertyMetadata(null));
    
    }      
}
