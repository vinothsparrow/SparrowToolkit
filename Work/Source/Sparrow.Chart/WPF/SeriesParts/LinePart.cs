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
    public class LinePart : LinePartBase
    {
        internal Line linePart;
        public LinePart()
        {

        }
        public LinePart(double x1,double x2,double y1,double y2)
        {
            this.X1 = x1;
            this.X2 = x2;
            this.Y1 = y1;
            this.Y2 = y2;
        }

        public LinePart(Point startPoint,Point endPoint)
        {
            this.X1 = startPoint.X;
            this.X2 = endPoint.X;
            this.Y1 = startPoint.Y;
            this.Y2 = endPoint.Y;
        }        

        public override UIElement CreatePart()
        {
            linePart = new Line();
            linePart.X1 = this.X1;
            linePart.X2 = this.X2;
            linePart.Y1 = this.Y1;
            linePart.Y2 = this.Y2;
            SetBindingForStrokeandStrokeThickness(linePart);
            return linePart;
        }
        public override void Refresh()
        {
            linePart.X1 = this.X1;
            linePart.X2 = this.X2;
            linePart.Y1 = this.Y1;
            linePart.Y2 = this.Y2;
        }
    }
}
