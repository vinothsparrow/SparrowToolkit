using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Data;
using System.Windows.Media;
using System.Collections;
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
    public class BubbleSeries : FillSeriesBase
    {
        private List<double> sizeValues;
        public BubbleSeries()
        {
            BubblePoints = new PointCollection();
            isFill = true;
            sizeValues = new List<double>();
        }

        public override void GenerateDatas()
        {
            BubblePoints.Clear();
            if (!isPointsGenerated)
                Parts.Clear();
            Point endPoint = new Point(0, 0);
            Point startPoint = new Point(0, 0);            
            if (PointsSource != null)
                sizeValues = this.GetReflectionValues(this.SizePath, PointsSource, sizeValues, false);

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
                        BubblePoints.Add(linePoint);
                        oldPoint = point;
                    }
                }
                if (this.RenderingMode == RenderingMode.Default)
                {
                    if (!isPointsGenerated)
                    {
                        for (int i = 0; i < BubblePoints.Count; i++)
                        {
                            ScatterPart scatterPart = new ScatterPart(BubblePoints[i]);
                            scatterPart.Size = sizeValues[i];
                            SetBindingForStrokeandStrokeThickness(scatterPart);
                            this.Parts.Add(scatterPart);
                        }
                        isPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        foreach (ScatterPart part in this.Parts)
                        {
                            part.X1 = BubblePoints[i].X;
                            part.Y1 = BubblePoints[i].Y;
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
            return new ScatterContainer();
        }

        public PointCollection BubblePoints
        {
            get { return (PointCollection)GetValue(BubblePointsPointsProperty); }
            set { SetValue(BubblePointsPointsProperty, value); }
        }

        public static readonly DependencyProperty BubblePointsPointsProperty =
            DependencyProperty.Register("BubblePointsPoints", typeof(PointCollection), typeof(BubbleSeries), new PropertyMetadata(null));



        public string SizePath
        {
            get { return (string)GetValue(SizePathProperty); }
            set { SetValue(SizePathProperty, value); }
        }

        public static readonly DependencyProperty SizePathProperty =
            DependencyProperty.Register("SizePath", typeof(string), typeof(BubbleSeries), new PropertyMetadata(string.Empty));

    }
}
