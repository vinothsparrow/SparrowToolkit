using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
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
    public class ColumnSeries : FillSeriesBase
    {        
        internal PointCollection startEndPoints;

        override public void GenerateDatas()
        {
            if (this.Points != null && this.seriesContainer != null)
            {
                ColumnPoints.Clear();
                if (!isPointsGenerated)
                    Parts.Clear();
                startEndPoints = new PointCollection();
                CalculateMinAndMax();
                ChartPoint oldPoint = new ChartPoint() { XValue = 0, YValue = 0 };
                IntializePoints();
                Point StartAndEndPoint = CalculateColumnSeriesInfo();
                foreach (ChartPoint point in this.Points)
                {
                    if (CheckValuePoint(oldPoint, point))
                    {
                        Point linePoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        Point startPoint = NormalizePoint(new Point(point.XValue + StartAndEndPoint.X, point.YValue));
                        Point endPoint = NormalizePoint(new Point(point.XValue + StartAndEndPoint.Y, yMin));
                        startEndPoints.Add(startPoint);
                        startEndPoints.Add(endPoint);
                        ColumnPoints.Add(linePoint);
                        oldPoint = point;
                    }
                }
                if (this.RenderingMode == RenderingMode.Default)
                {
                    //if (!UseSinglePart)
                    //{
                    if (!isPointsGenerated)
                    {
                        for (int i = 0; i <= this.startEndPoints.Count - 2; i += 2)
                        {
                            ColumnPart columnPart = new ColumnPart(startEndPoints[i].X, startEndPoints[i].Y, startEndPoints[i + 1].X, startEndPoints[i + 1].Y);
                            SetBindingForStrokeandStrokeThickness(columnPart);
                            this.Parts.Add(columnPart);
                            //}
                            //else
                            //{
                            //    LineSinglePart singlePart = new LineSinglePart(this.ColumnPoints);
                            //    SetBindingForStrokeandStrokeThickness(singlePart);
                            //    this.Parts.Add(singlePart);
                            //}
                        }
                        isPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        foreach (ColumnPart part in this.Parts)
                        {
                            part.X1 = startEndPoints[i].X;
                            part.Y1 = startEndPoints[i].Y;
                            part.X2 = startEndPoints[i + 1].X;
                            part.Y2 = startEndPoints[i + 1].Y;
                            part.Refresh();
                            i += 2;
                        }

                    }
                    
                }
                this.seriesContainer.Invalidate();                

            }
            isRefreshed = false;
        }

        public ColumnSeries()
        {
            ColumnPoints = new PointCollection();
            isFill = true;
        }

        internal override SeriesContainer CreateContainer()
        {
            return new ColumnContainer();
        }
        public Point CalculateColumnSeriesInfo()
        {
            double width = 0.8;
            double m_minPointsDelta = double.MaxValue;
            int position = Chart.columnSeries.IndexOf(this) + 1;
            int count = Chart.columnSeries.Count;
            foreach (SeriesBase series in Chart.Series)
            {
                List<double> xValues = series.xValues as List<double>;
                if (xValues != null)
                {
                    for (int i = 1; i < xValues.Count; i++)
                    {
                        double delta = xValues[i] - xValues[i - 1];
                        if (delta != 0)
                        {
                            m_minPointsDelta = Math.Min(m_minPointsDelta, delta);
                        }
                    }
                }
            }
            m_minPointsDelta = ((m_minPointsDelta == double.MaxValue || m_minPointsDelta >= 1 || m_minPointsDelta < 0) ? 1 : m_minPointsDelta);
            double div = m_minPointsDelta * width / count;
            double start = div * (position - 1) - m_minPointsDelta * width / 2;
            double end = start + div;
            return new Point(start,end);
            //}
        }      
        

        public PointCollection ColumnPoints
        {
            get { return (PointCollection)GetValue(ColumnPointsProperty); }
            set { SetValue(ColumnPointsProperty, value); }
        }

        public static readonly DependencyProperty ColumnPointsProperty =
            DependencyProperty.Register("ColumnPoints", typeof(PointCollection), typeof(ColumnSeries), new PropertyMetadata(null));
    }
}
