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
    /// Spline Series for Sparrow Charts
    /// </summary>
    public class SplineSeries : LineSeriesBase
    {
        public SplineSeries()
        {           
            SplinePoints = new PointCollection();
            ControlPoints = new PointCollection();
        }

        internal Point[] FirstControlPoints;
        internal Point[] SecondControlPoints;

        override public void GenerateDatas()
        {
            CalculateMinAndMax();
            ChartPoint oldPoint = new ChartPoint() { XValue = 0, YValue = 0 };
            IntializePoints();
            SplinePoints.Clear();
            if (!isPointsGenerated)
                Parts.Clear();
            if (this.Points != null && this.seriesContainer != null)
            {
                CalculateMinAndMax();
                foreach (ChartPoint point in this.Points)
                {
                    if (CheckValuePoint(oldPoint,point))
                    {
                        Point linePoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        this.SplinePoints.Add(linePoint);
                    }
                }
                if (this.SplinePoints.Count > 1)
                    BezierSpline.GetCurveControlPoints(this.SplinePoints.ToArray(), out FirstControlPoints, out SecondControlPoints);
                if (this.RenderingMode == RenderingMode.Default)
                {
                    if (!isPointsGenerated)
                    {
                        for (int i = 0; i < this.SplinePoints.Count - 1; i++)
                        {
                            SplinePart splinePart = new SplinePart(SplinePoints[i], FirstControlPoints[i], SecondControlPoints[i], SplinePoints[i + 1]);
                            SetBindingForStrokeandStrokeThickness(splinePart);
                            this.Parts.Add(splinePart);
                        }
                        isPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        foreach (SplinePart part in this.Parts)
                        {
                            part.startPoint = SplinePoints[i];
                            part.firstControlPoint = FirstControlPoints[i];
                            part.endControlPoint = SecondControlPoints[i];
                            part.endPoint = SplinePoints[i + 1];
                            part.Refresh();
                            i++;
                        }
                    }
                }
                this.seriesContainer.Invalidate();
            }
            isRefreshed = false;
        }
       
        internal override SeriesContainer CreateContainer()
        {
            return new SplineContainer();
        }

        public PointCollection SplinePoints
        {
            get { return (PointCollection)GetValue(SplinePointsProperty); }
            set { SetValue(SplinePointsProperty, value); }
        }

        public static readonly DependencyProperty SplinePointsProperty =
            DependencyProperty.Register("SplinePoints", typeof(PointCollection), typeof(SplineSeries), new PropertyMetadata(null));



        public PointCollection ControlPoints
        {
            get { return (PointCollection)GetValue(ControlPointsProperty); }
            set { SetValue(ControlPointsProperty, value); }
        }

        public static readonly DependencyProperty ControlPointsProperty =
            DependencyProperty.Register("ControlPoints", typeof(PointCollection), typeof(SplineSeries), new PropertyMetadata(null));
       
    }
}
