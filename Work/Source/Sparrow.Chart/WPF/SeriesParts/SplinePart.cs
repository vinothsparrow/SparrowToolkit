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
    public class SplinePart : LinePartBase
    {
        internal Point startPoint;
        internal Point firstControlPoint;
        internal Point endControlPoint;
        internal Point endPoint;
        internal Path splinePath;
        /// <summary>
        /// 
        /// </summary>
        public SplinePart()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="firstControlPoint"></param>
        /// <param name="endControlPoint"></param>
        /// <param name="endPoint"></param>
        public SplinePart(Point startPoint,Point firstControlPoint,Point endControlPoint,Point endPoint)
        {
            this.X1 = startPoint.X;
            this.Y1 = startPoint.Y;
            this.X2 = endPoint.X;
            this.Y2 = endPoint.Y;
            this.startPoint = startPoint;
            this.firstControlPoint = firstControlPoint;
            this.endControlPoint = endControlPoint;
            this.endPoint = endPoint;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override UIElement CreatePart()
        {
            splinePath = new Path();
            PathFigure figure = new PathFigure();
            BezierSegment bezierPoints = new BezierSegment();
            PathGeometry pathGeometry = new PathGeometry();
            figure.StartPoint = startPoint;
            bezierPoints.Point1 = firstControlPoint;
            bezierPoints.Point2 = endControlPoint;
            bezierPoints.Point3 = endPoint;
            figure.Segments.Add(bezierPoints);
            pathGeometry.Figures = new PathFigureCollection() { figure };
            splinePath.Data = pathGeometry;
            SetBindingForStrokeandStrokeThickness(splinePath);
            return splinePath;
        }

        public override void Refresh()
        {
            if (splinePath != null)
            {
                PathFigure figure = new PathFigure();
                BezierSegment bezierPoints = new BezierSegment();
                PathGeometry pathGeometry = new PathGeometry();
                figure.StartPoint = startPoint;
                bezierPoints.Point1 = firstControlPoint;
                bezierPoints.Point2 = endControlPoint;
                bezierPoints.Point3 = endPoint;
                figure.Segments.Add(bezierPoints);
                pathGeometry.Figures = new PathFigureCollection() { figure };
                splinePath.Data = pathGeometry;
            }
        }
    }
}
