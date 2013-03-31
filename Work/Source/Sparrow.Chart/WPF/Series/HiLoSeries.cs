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
    public class HiLoSeries : StockChartBase
    {       
        override public void GenerateDatas()
        {
            LowPoints.Clear();
            HighPoints.Clear();
            if (!isPointsGenerated)
                Parts.Clear();
            if (this.Points != null && this.seriesContainer != null)
            {
                CalculateMinAndMax();
                ChartPoint oldPoint = new ChartPoint() { XValue = 0, YValue = 0 };
                IntializePoints();
                int index = 0;
                foreach (ChartPoint point in this.Points)
                {
                    if (CheckValuePoint(oldPoint,point))
                    {
                        Point highPoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        Point lowPoint = NormalizePoint(new Point(lowPoints[index].XValue, lowPoints[index].YValue));
                        HighPoints.Add(highPoint);
                        LowPoints.Add(lowPoint);
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
                            LinePart linePart = new LinePart(this.HighPoints[i], this.LowPoints[i]);
                            SetBindingForStrokeandStrokeThickness(linePart);
                            this.Parts.Add(linePart);
                        }
                        isPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        foreach (LinePart part in this.Parts)
                        {
                            part.X1 = this.HighPoints[i].X;
                            part.Y1 = this.HighPoints[i].Y;
                            part.X2 = this.LowPoints[i].X;
                            part.Y2 = this.LowPoints[i].Y;
                            part.Refresh();
                            i++;
                        }
                    }

                }                
                this.seriesContainer.Invalidate();
            }

            isRefreshed = false;
        }

        public HiLoSeries()
        {
            HighPoints = new PointCollection();
            LowPoints = new PointCollection();
        }

        internal override SeriesContainer CreateContainer()
        {
            return new LineContainer();
        }
    }
}
