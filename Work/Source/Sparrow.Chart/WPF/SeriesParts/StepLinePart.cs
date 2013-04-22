using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
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
    /// 
    /// </summary>
    public class StepLinePart : LinePartBase
    {
        internal Point startPoint;
        internal Point endPoint;
        internal Point stepPoint;
        internal Polyline lines;
        /// <summary>
        /// 
        /// </summary>
        public StepLinePart()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="stepPoint"></param>
        /// <param name="endPoint"></param>
        public StepLinePart(Point startPoint, Point stepPoint, Point endPoint)
        {
            this.X1 = startPoint.X;
            this.Y1 = startPoint.Y;
            this.X2 = endPoint.X;
            this.Y2 = endPoint.Y;
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.stepPoint = stepPoint;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override UIElement CreatePart()
        {            
            lines = new Polyline();
            PointCollection pointsCollection=new PointCollection();
            pointsCollection.Add(startPoint);
            pointsCollection.Add(stepPoint);
            pointsCollection.Add(endPoint);
            lines.Points = pointsCollection;
            SetBindingForStrokeandStrokeThickness(lines);
            return lines;
        }

        public override void Refresh()
        {
            if (lines != null)
            {
                PointCollection pointsCollection = new PointCollection();
                pointsCollection.Add(startPoint);
                pointsCollection.Add(stepPoint);
                pointsCollection.Add(endPoint);
                lines.Points = pointsCollection;
            }
        }
        
    }
}
