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
            Binding strokeBinding = new Binding();
            strokeBinding.Source = this;
            strokeBinding.Path = new PropertyPath("Stroke");
            Binding strokeThicknessBinding = new Binding();
            strokeThicknessBinding.Path = new PropertyPath("StrokeThickness");
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

        /// <summary>
        /// Refresh the Series Part
        /// </summary>
        public virtual void Refresh()
        {
        }
    }
}
