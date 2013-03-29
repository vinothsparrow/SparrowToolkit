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
    public class FillPartBase : LinePartBase
    {
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(FillPartBase), new PropertyMetadata(null));
        protected override void SetBindingForStrokeandStrokeThickness(Shape shape)
        {
            Binding fillBinding = new Binding();
            fillBinding.Path = new PropertyPath("Fill");
            fillBinding.Source = this;
            shape.SetBinding(Shape.FillProperty, fillBinding);
            base.SetBindingForStrokeandStrokeThickness(shape);
        }

    }
}
