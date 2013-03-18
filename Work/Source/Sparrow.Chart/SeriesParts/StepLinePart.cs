using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sparrow.Chart
{
    /// <summary>
    /// 
    /// </summary>
    public class StepLinePart : LinePartBase
    {
        Point startPoint;
        Point endPoint;
        Point stepPoint;
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
            Polyline lines = new Polyline();
            PointCollection pointsCollection=new PointCollection();
            pointsCollection.Add(startPoint);
            pointsCollection.Add(stepPoint);
            pointsCollection.Add(endPoint);
            lines.Points = pointsCollection;
            SetBindingForStrokeandStrokeThickness(lines);
            return lines;
        }
        
    }
}
