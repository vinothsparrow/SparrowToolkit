using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if !WINRT
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
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
    public class LineSinglePartBase : SeriesPartBase
    {
        public LineSinglePartBase()
        {
        }

        private PointCollection linePoints;
        public PointCollection LinePoints 
        {
            get { return linePoints; }
            set { linePoints = value; }
        }

        
    }
}
