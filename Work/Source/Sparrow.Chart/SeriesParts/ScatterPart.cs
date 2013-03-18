using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;

namespace Sparrow.Chart
{
    public class ScatterPart : FillPartBase
    {

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(ScatterPart), new PropertyMetadata(0d));


        public ScatterPart()
        {
        }
        public ScatterPart(Point centerPoint)
        {
            this.X1 = centerPoint.X;
            this.X2 = centerPoint.X;
            this.Y1 = centerPoint.Y;
            this.Y2 = centerPoint.Y;
        }

        public override UIElement CreatePart()
        {
            Ellipse ellipse = new Ellipse();            
            Binding heightBinding = new Binding("Size");
            heightBinding.Source = this;
            ellipse.SetBinding(Ellipse.HeightProperty, heightBinding);
            Binding widthBinding = new Binding("Size");
            widthBinding.Source = this;
            ellipse.SetBinding(Ellipse.WidthProperty, widthBinding);

            SetBindingForStrokeandStrokeThickness(ellipse);
            return ellipse;
        }

    }
}
