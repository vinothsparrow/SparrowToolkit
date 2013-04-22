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
    public class ScatterPart : FillPartBase
    {
        internal Ellipse ellipse;

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
            ellipse = new Ellipse();            
            Binding heightBinding = new Binding();
            heightBinding.Path = new PropertyPath("Size");
            heightBinding.Source = this;
            ellipse.SetBinding(Ellipse.HeightProperty, heightBinding);
            Binding widthBinding = new Binding();
            widthBinding.Path = new PropertyPath("Size");
            widthBinding.Source = this;
            ellipse.SetBinding(Ellipse.WidthProperty, widthBinding);

            Canvas.SetLeft(ellipse, X1 - (ellipse.Width / 2));
            Canvas.SetTop(ellipse, Y1 - (ellipse.Height / 2));

            SetBindingForStrokeandStrokeThickness(ellipse);
            return ellipse;
        }

        public override void Refresh()
        {
            if (ellipse != null)
            {
                Canvas.SetLeft(ellipse, X1 - (ellipse.Width / 2));
                Canvas.SetTop(ellipse, Y1 - (ellipse.Height / 2));
            }
        }

    }
}
