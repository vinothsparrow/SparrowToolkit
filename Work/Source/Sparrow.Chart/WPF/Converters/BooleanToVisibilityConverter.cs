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
    /// Boolean to Visibility Converter
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
#if WINRT
         public object Convert(object value, Type targetType, object parameter, string language)
        {
#else
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
#endif
            if ((bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
            
        }
#if WINRT
         public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
#else
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
#endif
            return true;
        }
    }
}
