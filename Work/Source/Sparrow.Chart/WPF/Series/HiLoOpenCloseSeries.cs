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

namespace Sparrow.Chart
{
    public class HiLoOpenCloseSeries : StockChartBase
    {

        internal List<double> openValues;
        internal List<double> closeValues;
        internal PointsCollection openPoints;
        internal PointCollection openOffPoints;
        internal PointCollection closeOffPoints;
        internal PointsCollection closePoints;

        public override void GeneratePointsFromSource()
        {
            base.GeneratePointsFromSource();
            openValues = this.GetReflectionValues(this.OpenPath, PointsSource, openValues, false);
            closeValues = this.GetReflectionValues(this.ClosePath, PointsSource, closeValues, false);
            if (openValues != null && openValues.Count > 0)
            {
                this.openPoints = GetPointsFromValues(xValues, openValues);
            }
            if (closeValues != null && closeValues.Count > 0)
            {
                this.closePoints = GetPointsFromValues(xValues, closeValues);
            }
        }

        public override void GenerateDatas()
        {
            LowPoints.Clear();
            HighPoints.Clear();
            OpenPoints.Clear();
            ClosePoints.Clear();
            if (!isPointsGenerated)
                Parts.Clear();
            if (this.Points != null && this.seriesContainer != null)
            {
                CalculateMinAndMax();
                openOffPoints = new PointCollection();
                closeOffPoints = new PointCollection();
                ChartPoint oldPoint = new ChartPoint() { XValue = 0, YValue = 0 };
                IntializePoints();
                int index = 0;
                Point StartAndEndPoint = CalculateSeriesInfo();
                foreach (ChartPoint point in this.Points)
                {
                    if (CheckValuePoint(oldPoint, point))
                    {
                        Point highPoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        Point lowPoint = NormalizePoint(new Point(lowPoints[index].XValue, lowPoints[index].YValue));
                        Point openPoint = NormalizePoint(new Point(openPoints[index].XValue, openPoints[index].YValue));
                        Point closePoint = NormalizePoint(new Point(closePoints[index].XValue, closePoints[index].YValue));
                        Point openOffPoint = NormalizePoint(new Point(openPoints[index].XValue + StartAndEndPoint.X, openPoints[index].YValue));
                        Point closeOffPoint = NormalizePoint(new Point(closePoints[index].XValue - StartAndEndPoint.X, closePoints[index].YValue));
                        HighPoints.Add(highPoint);
                        LowPoints.Add(lowPoint);
                        OpenPoints.Add(openPoint);
                        ClosePoints.Add(closePoint);
                        openOffPoints.Add(openOffPoint);
                        closeOffPoints.Add(closeOffPoint);
                        oldPoint = point;
                    }
                    index++;
                }
                if (this.RenderingMode == RenderingMode.Default)
                {
                    if (!isPointsGenerated)
                    {
                        for (int i = 0; i < this.HighPoints.Count; i++)
                        {
                            HiLoOpenClosePart hiLoOpenClosePart = new HiLoOpenClosePart(this.HighPoints[i],this.LowPoints[i],this.ClosePoints[i],this.closeOffPoints[i],this.OpenPoints[i],this.openOffPoints[i]);
                            if (this.openPoints[i].YValue <= this.closePoints[i].YValue)
                                hiLoOpenClosePart.isBearfill = true;
                            else
                                hiLoOpenClosePart.isBearfill = false;
                            SetBindingForStrokeandStrokeThickness(hiLoOpenClosePart);
                            this.Parts.Add(hiLoOpenClosePart);
                        }
                        isPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        foreach (HiLoOpenClosePart part in this.Parts)
                        {
                            part.point1 = this.HighPoints[i];
                            part.point2 = this.LowPoints[i];
                            part.point3 = this.ClosePoints[i];
                            part.point4 = this.closeOffPoints[i];                            
                            part.point5 = this.OpenPoints[i];
                            part.point6 = this.openOffPoints[i];
                            part.Refresh();
                            i++;
                        }
                    }

                }
                this.seriesContainer.Invalidate();
            }

            isRefreshed = false;
        }

        public HiLoOpenCloseSeries()
        {
            HighPoints = new PointCollection();
            LowPoints = new PointCollection();
            OpenPoints = new PointCollection();
            ClosePoints = new PointCollection();
        }

        internal override SeriesContainer CreateContainer()
        {
            return new HiLoOpenCloseContainer();
        }

        protected override void SetBindingForStrokeandStrokeThickness(SeriesPartBase part)
        {
            HiLoOpenClosePart hiLoOpenClosePart = part as HiLoOpenClosePart;
            Binding strokeBinding = new Binding();
            if (hiLoOpenClosePart.isBearfill)
                strokeBinding.Path = new PropertyPath("BearFill");
            else
                strokeBinding.Path = new PropertyPath("BullFill");
            strokeBinding.Source = this;
            Binding strokeThicknessBinding = new Binding();
            strokeThicknessBinding.Path = new PropertyPath("StrokeThickness");
            strokeThicknessBinding.Source = this;
            part.SetBinding(SeriesPartBase.StrokeProperty, strokeBinding);
            part.SetBinding(SeriesPartBase.StrokeThicknessProperty, strokeThicknessBinding);            
        }
        public Point CalculateSeriesInfo()
        {
            double width = 1 - SparrowChart.GetSeriesMarginPercentage(this);
            double mininumWidth = double.MaxValue;
            int position = Chart.hiLoOpenCloseSeries.IndexOf(this) + 1;
            int count = Chart.hiLoOpenCloseSeries.Count;
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
                            mininumWidth = Math.Min(mininumWidth, delta);
                        }
                    }
                }
            }
            mininumWidth = ((mininumWidth == double.MaxValue || mininumWidth >= 1 || mininumWidth < 0) ? 1 : mininumWidth);
            double per = mininumWidth * width / count;
            double start = per * (position - 1) - mininumWidth * width / 2;
            double end = start + per;
            return new Point(start, end);
            //}
        }      
        public PointCollection OpenPoints
        {
            get { return (PointCollection)GetValue(OpenPointsProperty); }
            set { SetValue(OpenPointsProperty, value); }
        }

        public static readonly DependencyProperty OpenPointsProperty =
            DependencyProperty.Register("OpenPoints", typeof(PointCollection), typeof(HiLoOpenCloseSeries), new PropertyMetadata(null));



        public PointCollection ClosePoints
        {
            get { return (PointCollection)GetValue(ClosePointsProperty); }
            set { SetValue(ClosePointsProperty, value); }
        }

        public static readonly DependencyProperty ClosePointsProperty =
            DependencyProperty.Register("ClosePoints", typeof(PointCollection), typeof(HiLoOpenCloseSeries), new PropertyMetadata(null));



        public string OpenPath
        {
            get { return (string)GetValue(OpenPathProperty); }
            set { SetValue(OpenPathProperty, value); }
        }

        public static readonly DependencyProperty OpenPathProperty =
            DependencyProperty.Register("OpenPath", typeof(string), typeof(HiLoOpenCloseSeries), new PropertyMetadata(string.Empty));


        public string ClosePath
        {
            get { return (string)GetValue(ClosePathProperty); }
            set { SetValue(ClosePathProperty, value); }
        }

        public static readonly DependencyProperty ClosePathProperty =
            DependencyProperty.Register("ClosePath", typeof(string), typeof(HiLoOpenCloseSeries), new PropertyMetadata(null));


    }
}

