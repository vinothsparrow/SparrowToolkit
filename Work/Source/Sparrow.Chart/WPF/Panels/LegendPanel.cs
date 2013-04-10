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
    public class LegendPanel : StackPanel
    {


        public Dock DockPosition
        {
            get { return (Dock)GetValue(DockPositionProperty); }
            set { SetValue(DockPositionProperty, value); }
        }

        public static readonly DependencyProperty DockPositionProperty =
            DependencyProperty.Register("DockPosition", typeof(Dock), typeof(LegendPanel), new PropertyMetadata(Dock.Top,OnDockChanged));

        private static void OnDockChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as LegendPanel).DockChanged(args);
        }
        
        internal void DockChanged(DependencyPropertyChangedEventArgs args)
        {
            switch (DockPosition)
            {

                case Dock.Top:
                case Dock.Bottom:
                    this.Orientation = Orientation.Horizontal;
                    break;
                case Dock.Left:
                case Dock.Right:
                    this.Orientation = Orientation.Vertical;
                    break;
                default:
                    break;
            }
            this.InvalidateMeasure();
            this.InvalidateArrange();
        }        
    }
}
