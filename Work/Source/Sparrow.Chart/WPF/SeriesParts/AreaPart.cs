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
    public class AreaPart : FillPartBase
    {
        Point startPoint;
        Point areaStartPoint;
        Point areaEndPoint;
        Point endPoint;       

        public AreaPart()
        {
        }
        public AreaPart(Point startPoint, Point areaStartPoint, Point areaEndPoint, Point endPoint)
        {
            this.startPoint = startPoint;
            this.areaStartPoint = areaStartPoint;
            this.areaEndPoint = areaEndPoint;
            this.endPoint = endPoint;
            this.X1 = startPoint.X;
            this.Y1 = startPoint.Y;
            this.X2 = endPoint.X;
            this.Y2 = endPoint.Y;
        }
        public override UIElement CreatePart()
        {
            Path areaPath = new Path();
            PathFigure figure = new PathFigure();
            LineSegment startLineSegment = new LineSegment();
            LineSegment areaEndLineSegment = new LineSegment();
            LineSegment endLineSegment = new LineSegment();
            PathGeometry pathGeometry = new PathGeometry();
            figure.StartPoint = startPoint;            
            startLineSegment.Point = areaStartPoint;
            endLineSegment.Point = endPoint;
            areaEndLineSegment.Point = areaEndPoint;
            figure.Segments.Add(startLineSegment);
            figure.Segments.Add(areaEndLineSegment);
            figure.Segments.Add(endLineSegment);
            pathGeometry.Figures = new PathFigureCollection() { figure };
            areaPath.Data = pathGeometry;
            SetBindingForStrokeandStrokeThickness(areaPath);
            return areaPath;
        }       

    }
}
