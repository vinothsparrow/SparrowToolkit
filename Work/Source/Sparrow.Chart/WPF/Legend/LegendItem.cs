using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Data;
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
    /// Legend Item
    /// </summary>
    public class LegendItem : DependencyObject
    {
        public LegendItem()
        {            
        }

        public Shape Shape
        {
            get { return (Shape)GetValue(ShapeProperty); }
            set { SetValue(ShapeProperty, value); }
        }

        public static readonly DependencyProperty ShapeProperty =
            DependencyProperty.Register("Shape", typeof(Shape), typeof(LegendItem), new PropertyMetadata(null));


        public SeriesBase Series
        {
            get { return (SeriesBase)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series", typeof(SeriesBase), typeof(LegendItem), new PropertyMetadata(null,OnSeriesChanged));

        private static void OnSeriesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as LegendItem).SeriesChanged(args);
        }

        internal void SeriesChanged(DependencyPropertyChangedEventArgs args)
        {
            Binding binding = new Binding();
            binding.Source = Series;
            if (Series is FillSeriesBase)
                binding.Path = new PropertyPath("Fill");
            else
                binding.Path = new PropertyPath("Stroke");                    
            BindingOperations.SetBinding(this, LegendItem.IconColorProperty, binding);

            binding = new Binding();
            binding.Source = Series;
            binding.Path = new PropertyPath("Label");
            BindingOperations.SetBinding(this, LegendItem.LabelProperty, binding);
          
        }

        public Brush IconColor
        {
            get { return (Brush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register("IconColor", typeof(Brush), typeof(LegendItem), new PropertyMetadata(null));



        public object Label
        {
            get { return (object)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(object), typeof(LegendItem), new PropertyMetadata(null));



        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register("ShowIcon", typeof(bool), typeof(LegendItem), new PropertyMetadata(null));


        
    }
}
