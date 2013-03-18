using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Sparrow.Chart
{
    /// <summary>
    /// MajorTickLineSize to Thickness Converter
    /// </summary>
    public class MajorSizeThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double majorSize;
            majorSize = (double)parameter - (double)value;
           return majorSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0;
        }
    }
}
