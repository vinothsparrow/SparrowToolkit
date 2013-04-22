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
    /// <summary>
    /// Line Series for Sparrow Chart
    /// </summary>
    public class LineSeries : LineSeriesBase
    {
        override public void GenerateDatas()
        {            
            if (this.Points != null && this.seriesContainer != null)
            {
                LinePoints = new PointCollection();
                if (!isPointsGenerated)
                    Parts.Clear();
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
                if (this.RenderingMode == RenderingMode.Default)
                {
                    if (!isPointsGenerated)
                    {
                        if (!UseSinglePart)
                        {
                            for (int i = 0; i < this.LinePoints.Count - 1; i++)
                            {
                                if (CheckValue(this.LinePoints[i].X) && CheckValue(this.LinePoints[i].Y) && CheckValue(this.LinePoints[i + 1].X) && CheckValue(this.LinePoints[i + 1].Y))
                                {
                                    LinePart linePart = new LinePart(this.LinePoints[i], this.LinePoints[i + 1]);
                                    SetBindingForStrokeandStrokeThickness(linePart);
                                    this.Parts.Add(linePart);
                                }
                            }
                        }
                        else
                        {
                            LineSinglePart singlePart = new LineSinglePart(this.LinePoints);
                            SetBindingForStrokeandStrokeThickness(singlePart);
                            this.Parts.Add(singlePart);
                        }
                        isPointsGenerated = true;
                    }
                    else
                    {
                        int i=0;
                        if (!UseSinglePart)
                        {
                            foreach (LinePart part in this.Parts)
                            {
                                if (CheckValue(this.LinePoints[i].X) && CheckValue(this.LinePoints[i].Y) && CheckValue(this.LinePoints[i + 1].X) && CheckValue(this.LinePoints[i + 1].Y))
                                {
                                    part.X1 = this.LinePoints[i].X;
                                    part.Y1 = this.LinePoints[i].Y;
                                    part.X2 = this.LinePoints[i + 1].X;
                                    part.Y2 = this.LinePoints[i + 1].Y;
                                    part.Refresh();
                                }
                                i++;
                            }
                        }
                        else
                        {
                            foreach (LineSinglePart part in this.Parts)
                            {
                                part.LinePoints = this.LinePoints;
                                part.Refresh();
                                i++;
                            }
                        }
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
