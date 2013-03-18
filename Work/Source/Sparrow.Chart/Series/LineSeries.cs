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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace Sparrow.Chart
{
    /// <summary>
    /// Line Series for Sparrow Chart
    /// </summary>
    public class LineSeries : LineSeriesBase
    {
        override public void GenerateDatas()
        {
            LinePoints.Clear();
            Parts.Clear();
            if (this.Points != null && this.seriesContainer != null)
            {
                CalculateMinAndMax();
                ChartPoint oldPoint = new ChartPoint() { XValue = 0, YValue = 0 };
                IntializePoints();
                foreach (ChartPoint point in this.Points)
                {
                    if (CheckValuePoint(oldPoint,point))
                    {
                        Point linePoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        LinePoints.Add(linePoint);
                        oldPoint = point;
                    }
                }
                if (this.RenderingMode == RenderingMode.DefaultWPFRendering)
                {
                    if (!UseSinglePart)
                    {
                        for (int i = 0; i < this.LinePoints.Count - 1; i++)
                        {
                            LinePart linePart = new LinePart(this.LinePoints[i], this.LinePoints[i + 1]);
                            SetBindingForStrokeandStrokeThickness(linePart);
                            this.Parts.Add(linePart);
                        }
                    }
                    else
                    {
                        LineSinglePart singlePart = new LineSinglePart(this.LinePoints);
                        SetBindingForStrokeandStrokeThickness(singlePart);
                        this.Parts.Add(singlePart);
                    }
                }
                this.seriesContainer.Invalidate();
            }

            isRefreshed = false;
        }

        public LineSeries()
        {
            LinePoints = new PointCollection();
        }

        internal override SeriesContainer CreateContainer()
        {
            return new LineContainer();
        }


        public PointCollection LinePoints
        {
            get { return (PointCollection)GetValue(LinePointsProperty); }
            set { SetValue(LinePointsProperty, value); }
        }

        public static readonly DependencyProperty LinePointsProperty =
            DependencyProperty.Register("LinePoints", typeof(PointCollection), typeof(LineSeries), new PropertyMetadata(null));

    }

}
