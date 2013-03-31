using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Data;
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
    /// AreaSeries for SparrowChart
    /// </summary>
    public class AreaSeries : FillSeriesBase
    {
        public AreaSeries()
        {
            this.AreaPoints = new PointCollection();            
            this.isFill = true;
        }

        override public void GenerateDatas()
        {
            AreaPoints.Clear();
            if (!isPointsGenerated)
                Parts.Clear();
            Point endPoint = new Point(0,0);
            Point startPoint=new Point(0,0);
            int index = 0;
            if (this.Points != null && this.seriesContainer != null && this.Points.Count > 1)
            {
                CalculateMinAndMax();
                ChartPoint oldPoint = new ChartPoint() { XValue = 0, YValue = 0 };
                IntializePoints();
                AreaPoints.Add(startPoint);
                foreach (ChartPoint point in this.Points)
                {
                    if (CheckValuePoint(oldPoint,point))
                    {
                        Point linePoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        if (index == 0)
                            linePoint.X = linePoint.X - this.StrokeThickness;
                        AreaPoints.Add(linePoint);
                    }
                }
                if (this.RenderingMode == RenderingMode.Default)
                {
                    if (!isPointsGenerated)
                    {
                        for (int i = 0; i < AreaPoints.Count - 2; i++)
                        {
                            startPoint = NormalizePoint(new Point(this.Points[i].XValue, yMin));
                            endPoint = NormalizePoint(new Point(this.Points[i + 1].XValue, yMin));
                            AreaPart areaPart = new AreaPart(AreaPoints[i + 1], startPoint, endPoint, AreaPoints[i + 2]);
                            SetBindingForStrokeandStrokeThickness(areaPart);
                            this.Parts.Add(areaPart);

                        }
                        isPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        foreach (AreaPart part in this.Parts)
                        {
                            startPoint = NormalizePoint(new Point(this.Points[i].XValue, yMin));
                            endPoint = NormalizePoint(new Point(this.Points[i + 1].XValue, yMin));
                            part.startPoint = AreaPoints[i + 1];
                            part.areaStartPoint = startPoint;
                            part.areaEndPoint = endPoint;
                            part.endPoint = AreaPoints[i + 2];
                            part.Refresh();
                            i++;
                        }
                    }
                }
                endPoint = NormalizePoint(new Point(this.Points[this.Points.Count - 1].XValue, yMin));
                startPoint = NormalizePoint(new Point(xMin, yMin));
                startPoint.X = startPoint.X - this.StrokeThickness;                
                if (AreaPoints.Count > 0)
                {
                    AreaPoints[0] = startPoint;
                    AreaPoints.Add(endPoint);
                }

                if (this.seriesContainer != null)
                    this.seriesContainer.Invalidate();
            }
            isRefreshed = false;
        }

        internal override SeriesContainer CreateContainer()
        {
            return new AreaContainer();
        }
      
       
        public PointCollection AreaPoints
        {
            get { return (PointCollection)GetValue(AreaPointsProperty); }
            set { SetValue(AreaPointsProperty, value); }
        }

        public static readonly DependencyProperty AreaPointsProperty =
            DependencyProperty.Register("AreaPoints", typeof(PointCollection), typeof(AreaSeries), new PropertyMetadata(null));    

    }

   
}

