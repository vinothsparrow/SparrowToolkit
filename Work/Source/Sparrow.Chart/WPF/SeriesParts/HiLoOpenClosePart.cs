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
    public class HiLoOpenClosePart : LinePartBase
    {
        internal Point point1;
        internal Point point2;
        internal Point point3;
        internal Point point4;
        internal Point point5;
        internal Point point6;
        Path hiLoOpenClosePath;
        internal bool isBearfill;
        Canvas canvas;
        Line line1;
        Line line2;
        Line line3;

        public HiLoOpenClosePart()
        {

        }
        public HiLoOpenClosePart(Point point1, Point point2, Point point3, Point point4, Point point5, Point point6)
        {
            this.point1 = point1;
            this.X1 = point1.X;
            this.Y1 = point1.Y;
            this.point2 = point2;
            this.X2 = point2.X;
            this.Y2 = point2.Y;
            this.point3 = point3;
            this.X3 = point3.X;
            this.Y3 = point3.Y;
            this.point4 = point4;
            this.X4 = point4.X;
            this.Y4 = point4.Y;
            this.point5 = point5;
            this.X5 = point5.X;
            this.Y5 = point5.Y;
            this.point6 = point6;
            this.X6 = point6.X;
            this.Y6 = point6.Y;
        }

        private double x3;
        public double X3
        {
            get { return x3; }
            set { x3 = value; }
        }

        private double x4;
        public double X4
        {
            get { return x4; }
            set { x4 = value; }
        }

        private double x5;
        public double X5
        {
            get { return x5; }
            set { x5 = value; }
        }

        private double x6;
        public double X6
        {
            get { return x6; }
            set { x6 = value; }
        }

        private double y3;
        public double Y3
        {
            get { return y3; }
            set { y3 = value; }
        }

        private double y4;
        public double Y4
        {
            get { return y4; }
            set { y4 = value; }
        }
        private double y5;
        public double Y5
        {
            get { return y5; }
            set { y5 = value; }
        }

        private double y6;
        public double Y6
        {
            get { return y6; }
            set { y6 = value; }
        }
        
        public override UIElement CreatePart()
        {
            canvas = new Canvas();
            line1 = new Line();
            line2 = new Line();
            line3 = new Line();

            line1.X1 = point1.X;
            line1.X2 = point2.X;
            line1.Y1 = point1.Y;
            line1.Y2 = point2.Y;

            line2.X1 = point3.X;
            line2.X2 = point4.X;
            line2.Y1 = point3.Y;
            line2.Y2 = point4.Y;

            line3.X1 = point5.X;
            line3.X2 = point6.X;
            line3.Y1 = point5.Y;
            line3.Y2 = point6.Y;

            canvas.Children.Add(line1);
            canvas.Children.Add(line2);
            canvas.Children.Add(line3);
            
            SetBindingForStrokeandStrokeThickness(line1);
            SetBindingForStrokeandStrokeThickness(line2);
            SetBindingForStrokeandStrokeThickness(line3);

            return canvas;
        }

        public override void Refresh()
        {
            line1.X1 = point1.X;
            line1.X2 = point2.X;
            line1.Y1 = point1.Y;
            line1.Y2 = point2.Y;

            line2.X1 = point3.X;
            line2.X2 = point4.X;
            line2.Y1 = point3.Y;
            line2.Y2 = point4.Y;

            line3.X1 = point5.X;
            line3.X2 = point6.X;
            line3.Y1 = point5.Y;
            line3.Y2 = point6.Y;
        }
    }
}
