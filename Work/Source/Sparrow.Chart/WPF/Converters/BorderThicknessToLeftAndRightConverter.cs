using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// BorderThickness to XAxis Left and Right Converter
    /// </summary>
    public class BorderThicknessToLeftAndRightConverter : IValueConverter
    {
#if WINRT
        public object Convert(object value, Type targetType, object parameter, string language)
        {
#else
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
#endif
            if (value is Thickness)
            {
                Thickness borderThickness = (Thickness)value;
                return new Thickness(borderThickness.Left, 0, borderThickness.Right, 0);
            }
            else
                return null;
        }

#if WINRT
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
#else
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
#endif
            return new Thickness(0);
        }
    }
}
