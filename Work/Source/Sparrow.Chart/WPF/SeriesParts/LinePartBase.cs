using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Sparrow.Chart
{
    public class LinePartBase : SeriesPartBase
    {
       
        public LinePartBase()
        {
        }

        private double x1;
        public double X1
        {
            get { return x1; }
            set { x1 = value; }
        }

        private double x2;
        public double X2
        {
            get { return x2; }
            set { x2 = value; }
        }

        private double y1;
        public double Y1
        {
            get { return y1; }
            set { y1 = value; }
        }

        private double y2;
        public double Y2
        {
            get { return y2; }
            set { y2 = value; }
        }


    }
}
