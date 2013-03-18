using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Sparrow.Chart
{
    /// <summary>
    /// BorderThickness to YAXis Top and Bottom Converter
    /// </summary>
    public class BorderThicknessToTopBottomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Thickness)
            {
                Thickness borderThickness = (Thickness)value;
                return new Thickness(0, borderThickness.Top, 0, borderThickness.Bottom);
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Thickness(0);
        }
    }
}
