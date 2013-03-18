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
    /// <summary>
    /// 
    /// </summary>
    public class SeriesPartBase : FrameworkElement
    {
        /// <summary>
        /// 
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(SeriesPartBase), new PropertyMetadata(null));

        /// <summary>
        /// 
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
          DependencyProperty.Register("StrokeThickness", typeof(double), typeof(SeriesPartBase), new PropertyMetadata(1d));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shape"></param>
        protected virtual void SetBindingForStrokeandStrokeThickness(Shape shape)
        {
            Binding strokeBinding = new Binding("Stroke");
            strokeBinding.Source = this;
            
            Binding strokeThicknessBinding = new Binding("StrokeThickness");
            strokeThicknessBinding.Source = this;
            shape.SetBinding(Shape.StrokeProperty, strokeBinding);
            shape.SetBinding(Shape.StrokeThicknessProperty, strokeThicknessBinding);
        }

        /// <summary>
        /// Create a visual for single Series Part
        /// </summary>
        /// <returns>UIElement</returns>
        public virtual UIElement CreatePart()
        {
            return null;
        }

    }
}
