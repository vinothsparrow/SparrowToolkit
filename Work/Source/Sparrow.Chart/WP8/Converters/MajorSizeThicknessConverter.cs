using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Data;
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
    /// MajorTickLineSize to Thickness Converter
    /// </summary>
    public class MajorSizeThicknessConverter : IValueConverter
    {
#if WINRT
        public object Convert(object value, Type targetType, object parameter, string language)
        {
#else
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
#endif
            double majorSize;
            majorSize = (double)parameter - (double)value;
           return majorSize;
        }

#if WINRT
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
#else
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
#endif
            return 0;
        }
    }
}
