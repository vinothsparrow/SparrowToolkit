using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

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
            Binding fillBinding = new Binding("Fill");
            fillBinding.Source = this;
            part.SetBinding(FillPartBase.FillProperty, fillBinding);
            base.SetBindingForStrokeandStrokeThickness(part);
        }
    }
}
