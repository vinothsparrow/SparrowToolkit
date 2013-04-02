﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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
    public class LineSinglePart : LineSinglePartBase
    {
        internal Polyline linePart;
        public LineSinglePart()
        {
        }

        public LineSinglePart(PointCollection points)
        {
            this.LinePoints = points;
        }

        public override UIElement CreatePart()
        {
            linePart = new Polyline();
            linePart.Points = this.LinePoints;            
            SetBindingForStrokeandStrokeThickness(linePart);
            return linePart;
        }
        public override void Refresh()
        {
            linePart.Points = this.LinePoints;     
        }
    }
}