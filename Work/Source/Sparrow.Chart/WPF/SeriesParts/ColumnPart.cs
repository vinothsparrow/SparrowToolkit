using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
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
    public class ColumnPart : FillPartBase
    {       
        public double partWidth;
        public double partHeight;
        public double columnMargin;
        internal Rectangle rectPart;

        public ColumnPart()
        {
        }

        public ColumnPart(double x, double y,double x2,double y2)
        {
            this.X1 = x;
            this.Y1 = y;
            this.X2 = x2;
            this.Y2 = y2;
        }

        private double x1;
        public double X1
        {
            get { return x1; }
            set { x1 = value; }
        }

        private double x2;
        public double X2
        {
            get { return x2; }
            set { x2 = value; }
        }

        private double y1;
        public double Y1
        {
            get { return y1; }
            set { y1 = value; }
        }

        private double y2;
        public double Y2
        {
            get { return y2; }
            set { y2 = value; }
        }
        public override UIElement CreatePart()
        {
            Rect rect = new Rect(new Point(X1, Y1), new Point(X2, Y2));
            rectPart = new Rectangle();
            rectPart.Height = rect.Height;
            rectPart.Width = rect.Width;
            rectPart.SetValue(Canvas.LeftProperty, rect.X);
            rectPart.SetValue(Canvas.TopProperty, rect.Y);
            SetBindingForStrokeandStrokeThickness(rectPart);
            return rectPart;
        }

        public override void Refresh()
        {
            Rect rect = new Rect(new Point(X1, Y1), new Point(X2, Y2));
            rectPart.Height = rect.Height;
            rectPart.Width = rect.Width;
            rectPart.SetValue(Canvas.LeftProperty, rect.X);
            rectPart.SetValue(Canvas.TopProperty, rect.Y);
        }
    }
}
