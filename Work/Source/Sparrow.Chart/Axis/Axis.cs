using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Sparrow.Chart
{

    public interface Axis
    {        

        object MinValue
        {
            get;
            set;
        }

        object MaxValue
        {
            get;
            set;
        }

        object Interval
        {
            get;
            set;
        }

        Style AxisLineStyle
        {
            get;
            set;
        }

        SeriesCollection Series
        {
            get;
            set;
        }

        
    }
}
