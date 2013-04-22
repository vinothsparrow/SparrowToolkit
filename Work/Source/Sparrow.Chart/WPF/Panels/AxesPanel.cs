using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
#if WPF
using System.Drawing;
using System.Drawing.Drawing2D;
#endif
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;
using Image = System.Windows.Controls.Image;
using System.Windows.Threading;
using Size = System.Windows.Size;
using System.Windows.Media.Animation;
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

using System.IO;

namespace Sparrow.Chart
{
    public class AxesPanel : StackPanel
    {

        public Axes Axes
        {
            get { return (Axes)GetValue(AxesProperty); }
            set { SetValue(AxesProperty, value); }
        }

        public static readonly DependencyProperty AxesProperty =
            DependencyProperty.Register("Axes", typeof(Axes), typeof(AxesPanel), new PropertyMetadata(null));


    }
}
