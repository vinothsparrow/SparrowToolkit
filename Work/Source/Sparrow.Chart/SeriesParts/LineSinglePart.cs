using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sparrow.Chart
{
    public class LineSinglePart : LineSinglePartBase
    {
        public LineSinglePart()
        {
        }

        public LineSinglePart(PointCollection points)
        {
            this.LinePoints = points;
        }

        public override UIElement CreatePart()
        {
            Polyline linePart = new Polyline();
            linePart.Points = this.LinePoints;            
            SetBindingForStrokeandStrokeThickness(linePart);
            return linePart;
        }
    }
}
