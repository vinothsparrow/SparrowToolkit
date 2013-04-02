using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightDemos
{
    public class Model
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Size { get; set; }
        public Model(double x, double y, double size)
        {
            X = x;
            Y = y;
            Size = size;
        }
    }
}
