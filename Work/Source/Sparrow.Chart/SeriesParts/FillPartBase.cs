using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

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
            Binding fillBinding = new Binding("Fill");
            fillBinding.Source = this;
            shape.SetBinding(Shape.FillProperty, fillBinding);
            base.SetBindingForStrokeandStrokeThickness(shape);
        }

    }
}
