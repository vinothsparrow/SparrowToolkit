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
    /// BorderThickness to XAxis Left and Right Converter
    /// </summary>
    public class BorderThicknessToLeftAndRightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Thickness)
            {
                Thickness borderThickness = (Thickness)value;
                return new Thickness(borderThickness.Left, 0, borderThickness.Right, 0);
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
