using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Sparrow.Chart
{
    /// <summary>
    /// Legend Item
    /// </summary>
    public class LegendItem : ContentControl
    {
        public LegendItem()
        {
            this.DefaultStyleKey = typeof(LegendItem);
        }

        public Shape Shape
        {
            get { return (Shape)GetValue(ShapeProperty); }
            set { SetValue(ShapeProperty, value); }
        }

        public static readonly DependencyProperty ShapeProperty =
            DependencyProperty.Register("Shape", typeof(Shape), typeof(LegendItem), new PropertyMetadata(null));
        
    }
}
