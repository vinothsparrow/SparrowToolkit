using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Data;
using System.Windows.Media;
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
    /// Fill Based Series Such as Area,Scatter and Column Series
    /// </summary>
    public class FillSeriesBase : LineSeriesBase
    {
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(FillSeriesBase), new PropertyMetadata(null));


        protected override void SetBindingForStrokeandStrokeThickness(SeriesPartBase part)
        {           
            Binding fillBinding = new Binding();
            fillBinding.Path = new PropertyPath("Fill");
            fillBinding.Source = this;
            part.SetBinding(FillPartBase.FillProperty, fillBinding);
            base.SetBindingForStrokeandStrokeThickness(part);
        }
    }
}
